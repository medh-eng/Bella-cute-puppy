"""
Bella Python Auto-Rigger
Runs entirely in Python (no Blender needed).
Outputs a skinned GLB ready for Unity.
"""
import pygltflib, numpy as np, struct, io, json, copy, math
from pygltflib import GLTF2, Skin, Node, Accessor, BufferView, Buffer, \
                      Animation, AnimationChannel, AnimationSampler, \
                      AnimationChannelTarget
from pygltflib.validator import validate

GLB_IN  = "/home/claude/Bella-cute-puppy/bella_raw.glb"
GLB_OUT = "/home/claude/Bella-cute-puppy/unity-project/Assets/Art/Bella/Model/bella_rigged.glb"

# ── 1. LOAD & DECODE ──────────────────────────────────────────────────────────
glb  = GLTF2().load(GLB_IN)
blob = bytearray(glb.binary_blob())

def get_array(acc_idx, dtype_map={5120:np.int8,5121:np.uint8,5122:np.int16,
                                   5123:np.uint16,5125:np.uint32,5126:np.float32},
              comp_map={'SCALAR':1,'VEC2':2,'VEC3':3,'VEC4':4,'MAT2':4,'MAT3':9,'MAT4':16}):
    acc  = glb.accessors[acc_idx]
    bv   = glb.bufferViews[acc.bufferView]
    off  = (bv.byteOffset or 0) + (acc.byteOffset or 0)
    dt   = dtype_map[acc.componentType]
    nc   = comp_map[acc.type]
    raw  = blob[off : off + acc.count * nc * np.dtype(dt).itemsize]
    arr  = np.frombuffer(raw, dtype=dt).reshape(acc.count, nc).copy()
    return arr.astype(np.float32) if dt == np.float32 else arr

prim   = glb.meshes[0].primitives[0]
verts  = get_array(prim.attributes.POSITION)         # (N,3) float32
N      = len(verts)
print(f"Loaded {N} vertices, {N//3} tris (approx)")

X0,X1 = float(verts[:,0].min()), float(verts[:,0].max())
Y0,Y1 = float(verts[:,1].min()), float(verts[:,1].max())
Z0,Z1 = float(verts[:,2].min()), float(verts[:,2].max())
W,H,D = X1-X0, Y1-Y0, Z1-Z0

# ── 2. BONE DEFINITIONS ───────────────────────────────────────────────────────
# (name, head_xyz, tail_xyz, parent_idx)
# Y-up. Dog nose → -Z, tail → +Z (matches Meshy output).

FLOOR  = Y0;           PAW_Y   = Y0 + H*0.03
KNEE_Y = Y0 + H*0.22;  BELLY   = Y0 + H*0.32
BACK   = Y0 + H*0.57;  NECK_B  = Y0 + H*0.66
HEAD_C = Y0 + H*0.80;  HEAD_T  = Y1

FRONT_Z= Z0 + D*0.26;  BACK_Z  = Z1 - D*0.22
MID_Z  = (FRONT_Z+BACK_Z)*0.5
LX     = (X1-X0)*0.30   # half-spacing for legs

# (name, head, tail, parent)  parent=-1 → root
BONES = [
    # 0  root (virtual, at origin of whole rig)
    ("Root",           (0, BELLY,  MID_Z),      (0, BELLY+0.03, MID_Z),    -1),
    # 1  hips
    ("Hips",           (0, BACK,   BACK_Z),     (0, BACK,       MID_Z),    0),
    # 2  spine
    ("Spine",          (0, BACK,   MID_Z),      (0, BACK,       FRONT_Z),  1),
    # 3  chest
    ("Chest",          (0, BACK,   FRONT_Z),    (0, BACK+0.02,  FRONT_Z-D*0.08), 2),
    # 4  neck
    ("Neck",           (0, NECK_B, FRONT_Z-D*0.08),(0,NECK_B+H*0.07,FRONT_Z-D*0.16), 3),
    # 5  head
    ("Head",           (0, NECK_B+H*0.07, FRONT_Z-D*0.16),(0,HEAD_C,Z0+D*0.06), 4),
    # 6  head end
    ("Head_End",       (0, HEAD_C, Z0+D*0.06),  (0, HEAD_T,     Z0),       5),
    # 7  tail 1
    ("Tail_1",         (0, BACK,   Z1),         (0, BACK+H*0.08,Z1+D*0.06), 1),
    # 8  tail 2
    ("Tail_2",         (0, BACK+H*0.08,Z1+D*0.06),(0,BACK+H*0.15,Z1+D*0.11), 7),
    # 9  tail 3
    ("Tail_3",         (0, BACK+H*0.15,Z1+D*0.11),(0,BACK+H*0.22,Z1+D*0.14), 8),
    # 10 FL shoulder
    ("FrontL_Shoulder",(-LX, BACK,   FRONT_Z),  (-LX, BELLY, FRONT_Z),     3),
    # 11 FL upper
    ("FrontL_Upper",   (-LX, BELLY,  FRONT_Z),  (-LX, KNEE_Y, FRONT_Z+D*0.03), 10),
    # 12 FL lower
    ("FrontL_Lower",   (-LX, KNEE_Y, FRONT_Z+D*0.03),(-LX, PAW_Y, FRONT_Z), 11),
    # 13 FL paw
    ("FrontL_Paw",     (-LX, PAW_Y,  FRONT_Z),  (-LX, PAW_Y, FRONT_Z-D*0.05), 12),
    # 14 FR shoulder
    ("FrontR_Shoulder",(+LX, BACK,   FRONT_Z),  (+LX, BELLY, FRONT_Z),     3),
    # 15 FR upper
    ("FrontR_Upper",   (+LX, BELLY,  FRONT_Z),  (+LX, KNEE_Y, FRONT_Z+D*0.03), 14),
    # 16 FR lower
    ("FrontR_Lower",   (+LX, KNEE_Y, FRONT_Z+D*0.03),(+LX, PAW_Y, FRONT_Z), 15),
    # 17 FR paw
    ("FrontR_Paw",     (+LX, PAW_Y,  FRONT_Z),  (+LX, PAW_Y, FRONT_Z-D*0.05), 16),
    # 18 BL hip
    ("BackL_Hip",      (-LX, BACK,   BACK_Z),   (-LX, BELLY, BACK_Z),      1),
    # 19 BL upper
    ("BackL_Upper",    (-LX, BELLY,  BACK_Z),   (-LX, KNEE_Y, BACK_Z-D*0.04), 18),
    # 20 BL lower
    ("BackL_Lower",    (-LX, KNEE_Y, BACK_Z-D*0.04),(-LX, PAW_Y, BACK_Z), 19),
    # 21 BL paw
    ("BackL_Paw",      (-LX, PAW_Y,  BACK_Z),   (-LX, PAW_Y, BACK_Z+D*0.05), 20),
    # 22 BR hip
    ("BackR_Hip",      (+LX, BACK,   BACK_Z),   (+LX, BELLY, BACK_Z),      1),
    # 23 BR upper
    ("BackR_Upper",    (+LX, BELLY,  BACK_Z),   (+LX, KNEE_Y, BACK_Z-D*0.04), 22),
    # 24 BR lower
    ("BackR_Lower",    (+LX, KNEE_Y, BACK_Z-D*0.04),(+LX, PAW_Y, BACK_Z), 23),
    # 25 BR paw
    ("BackR_Paw",      (+LX, PAW_Y,  BACK_Z),   (+LX, PAW_Y, BACK_Z+D*0.05), 24),
]
NB = len(BONES)
print(f"Defined {NB} bones")

# ── 3. COMPUTE SKINNING WEIGHTS ───────────────────────────────────────────────
# For each vertex, find the 4 bones whose midpoints are closest.
# Weight = 1/distance^2, normalised.

bone_mids = np.array([
    [(h[0]+t[0])/2, (h[1]+t[1])/2, (h[2]+t[2])/2]
    for _, h, t, _ in BONES
], dtype=np.float32)                   # (NB, 3)

# Distance from every vertex to every bone midpoint: (N, NB)
diff = verts[:, None, :] - bone_mids[None, :, :]   # (N, NB, 3)
dist = np.linalg.norm(diff, axis=2) + 1e-6          # (N, NB)

# Top-4 closest bones per vertex
top4_idx = np.argsort(dist, axis=1)[:, :4].astype(np.uint16)   # (N, 4)
top4_w   = 1.0 / (dist[np.arange(N)[:, None], top4_idx] ** 2)
top4_w  /= top4_w.sum(axis=1, keepdims=True)                    # normalize
top4_w   = top4_w.astype(np.float32)

print(f"Weights computed — sample vertex 0: bones={top4_idx[0]}, w={top4_w[0]}")

# ── 4. PACK NEW BINARY DATA ───────────────────────────────────────────────────
new_bin = bytearray()

def add_data(arr, pad=True):
    """Append array bytes to new_bin, return byte offset."""
    raw = arr.tobytes()
    off = len(new_bin)
    new_bin.extend(raw)
    if pad and len(new_bin) % 4 != 0:
        new_bin.extend(b'\x00' * (4 - len(new_bin) % 4))
    return off, len(raw)

# Copy original binary data first (keep existing accessors valid)
orig_len = len(blob)
new_bin.extend(blob)
if len(new_bin) % 4: new_bin.extend(b'\x00' * (4 - len(new_bin) % 4))

# JOINTS_0 accessor
j_off, j_sz = add_data(top4_idx)
# WEIGHTS_0 accessor
w_off, w_sz = add_data(top4_w)

# Inverse bind matrices: identity matrix per bone, shifted by -bone_head
# Column-major 4x4: [ col0 col1 col2 col3 ]
IBM_list = []
for _, head, _, _ in BONES:
    tx, ty, tz = head
    # inv-bind = inverse of bone's global bind transform (pure translation)
    m = [1,0,0,0,  0,1,0,0,  0,0,1,0,  -tx,-ty,-tz,1]
    IBM_list.append(m)

ibm_arr = np.array(IBM_list, dtype=np.float32)   # (NB, 16)
ibm_off, ibm_sz = add_data(ibm_arr)

print(f"Binary packed: joints@{j_off}, weights@{w_off}, ibm@{ibm_off}")

# ── 5. ADD BUFFER VIEWS & ACCESSORS ──────────────────────────────────────────
BUF_IDX = 0   # single buffer

def add_bv(offset, length, target=None):
    bv = pygltflib.BufferView(buffer=BUF_IDX, byteOffset=offset,
                               byteLength=length, target=target)
    glb.bufferViews.append(bv)
    return len(glb.bufferViews) - 1

def add_acc(bv_idx, comp_type, count, acc_type, min_v=None, max_v=None):
    acc = pygltflib.Accessor(bufferView=bv_idx, byteOffset=0,
                              componentType=comp_type, count=count, type=acc_type)
    if min_v: acc.min = min_v
    if max_v: acc.max = max_v
    glb.accessors.append(acc)
    return len(glb.accessors) - 1

j_bv  = add_bv(j_off,   j_sz)
w_bv  = add_bv(w_off,   w_sz)
ibm_bv= add_bv(ibm_off, ibm_sz)

j_acc  = add_acc(j_bv,   5123, N,  "VEC4")     # UNSIGNED_SHORT
w_acc  = add_acc(w_bv,   5126, N,  "VEC4")     # FLOAT
ibm_acc= add_acc(ibm_bv, 5126, NB, "MAT4")

prim.attributes.JOINTS_0  = j_acc
prim.attributes.WEIGHTS_0 = w_acc

# ── 6. ADD BONE NODES ─────────────────────────────────────────────────────────
first_bone_node = len(glb.nodes)

def v3(xyz): return list(xyz)

for i, (name, head, _, parent_i) in enumerate(BONES):
    node = pygltflib.Node(name=name, translation=v3(head))
    glb.nodes.append(node)

# Wire children
for i, (_, _, _, parent_i) in enumerate(BONES):
    if parent_i >= 0:
        pn = glb.nodes[first_bone_node + parent_i]
        if pn.children is None: pn.children = []
        pn.children.append(first_bone_node + i)

# ── 7. CREATE SKIN ────────────────────────────────────────────────────────────
skin = pygltflib.Skin(
    name="BellaSkin",
    inverseBindMatrices=ibm_acc,
    joints=[first_bone_node + i for i in range(NB)],
    skeleton=first_bone_node       # Root bone
)
glb.skins.append(skin)
mesh_node_idx = 0    # the original mesh node
glb.nodes[mesh_node_idx].skin = 0

# ── 8. ADD ROOT BONE TO SCENE ─────────────────────────────────────────────────
scene = glb.scenes[glb.scene or 0]
if scene.nodes is None: scene.nodes = []
scene.nodes.append(first_bone_node)   # Root bone enters the scene

# ── 9. UPDATE BUFFER SIZE ────────────────────────────────────────────────────
glb.buffers[0].byteLength = len(new_bin)

# ── 10. SAVE ──────────────────────────────────────────────────────────────────
glb.set_binary_blob(bytes(new_bin))
glb.save(GLB_OUT)
print(f"\n✅ Rigged GLB saved → {GLB_OUT}")
print(f"   Size: {len(new_bin)/1024/1024:.1f} MB  |  Bones: {NB}")
print("   Import this file into Unity as a 3D model.")
