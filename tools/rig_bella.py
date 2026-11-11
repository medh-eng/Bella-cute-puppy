"""
Bella Auto-Rigger — Blender 3.6 / 4.x Python script
=====================================================
HOW TO USE (takes about 60 seconds):
  1. Install Blender free from blender.org (any recent version works)
  2. Open Blender → delete the default cube (X → Delete)
  3. Top menu → Scripting tab
  4. Click "New" to create a new script
  5. Paste this ENTIRE file into the text editor
  6. Click "▶ Run Script"  (or press Alt+P)
  7. Wait ~30 seconds for bones + weight paint to finish
  8. File → Export → FBX (.fbx)
        ✅ Selected Objects
        ✅ Armature
        ✅ Mesh
        Path: Assets/Art/Bella/Animations/bella_rigged.fbx
  9. Done!  Import the FBX in Unity.

WHAT IT DOES:
  - Imports bella.glb from the same folder as this script
  - Builds a 22-bone quadruped skeleton tuned to this dog's bounding box
  - Parents mesh to armature with Automatic Weights
  - Creates a simple idle animation (breathing + tail wag) as a preview

Bone hierarchy:
  ROOT
  └─ Hips
     ├─ Spine → Chest → Neck → Head → Head_End
     ├─ Tail_1 → Tail_2 → Tail_3 → Tail_End
     ├─ FrontL_Shoulder → FrontL_Upper → FrontL_Lower → FrontL_Paw
     ├─ FrontR_Shoulder → FrontR_Upper → FrontR_Lower → FrontR_Paw
     ├─ BackL_Hip → BackL_Upper → BackL_Lower → BackL_Paw
     └─ BackR_Hip → BackR_Upper → BackR_Lower → BackR_Paw
"""

import bpy, math, os

# ── 0. SETUP ──────────────────────────────────────────────────────────────────

bpy.ops.object.select_all(action='SELECT')
bpy.ops.object.delete()

SCRIPT_DIR = os.path.dirname(os.path.realpath(__file__))
GLB_PATH   = os.path.join(SCRIPT_DIR, "..", "unity-project",
                           "Assets", "Art", "Bella", "Model", "bella.glb")

# ── 1. IMPORT GLB ──────────────────────────────────────────────────────────────

bpy.ops.import_scene.gltf(filepath=GLB_PATH)
mesh_obj = next(o for o in bpy.context.scene.objects if o.type == 'MESH')
print(f"Imported mesh: {mesh_obj.name}")

# Center and place on floor
bpy.context.view_layer.objects.active = mesh_obj
bpy.ops.object.origin_set(type='ORIGIN_GEOMETRY', center='BOUNDS')
mesh_obj.location = (0, 0, 0)

# Get updated bounding box in world space
bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
bb = mesh_obj.bound_box          # 8 corners in local space (after apply)
xs = [v[0] for v in bb]; ys = [v[1] for v in bb]; zs = [v[2] for v in bb]
X0,X1 = min(xs),max(xs)         # left / right
Y0,Y1 = min(ys),max(ys)         # bottom / top
Z0,Z1 = min(zs),max(zs)         # front / back (Blender: front = -Y, but Meshy uses -Z)

W  = X1-X0; H = Y1-Y0; D = Z1-Z0
HW = W/2;   HH= H/2;   HD= D/2

# Key heights
FLOOR     = Y0                   # bottom of paws
BELLY     = Y0 + H*0.30          # belly
BACK      = Y0 + H*0.55          # top of back (spine)
NECK_BASE = Y0 + H*0.65
HEAD_CTR  = Y0 + H*0.82
HEAD_TOP  = Y1

# Fore-aft splits (Z0 = front/nose, Z1 = back/tail in Meshy output)
FRONT_Z   = Z0 + D*0.25          # front leg column
BACK_Z    = Z1 - D*0.20          # back leg column
SPINE_MID = (FRONT_Z + BACK_Z)/2

LEG_X     = HW * 0.65            # half-width for leg placement
PAW_Y     = FLOOR + H*0.02       # just off the floor
KNEE_Y    = FLOOR + H*0.20
HIP_Y     = BELLY

print(f"Dog bounds  X:{X0:.2f}→{X1:.2f}  Y:{Y0:.2f}→{Y1:.2f}  Z:{Z0:.2f}→{Z1:.2f}")

# ── 2. CREATE ARMATURE ─────────────────────────────────────────────────────────

bpy.ops.object.armature_add(enter_editmode=True, location=(0,0,0))
arm_obj = bpy.context.active_object
arm_obj.name = "Bella_Armature"
arm     = arm_obj.data
arm.name= "Bella_Rig"

def bone(name, head, tail, parent=None, conn=False):
    """Create an edit bone."""
    b = arm.edit_bones.new(name)
    b.head = head; b.tail = tail
    b.use_connect = conn
    if parent:
        b.parent = arm.edit_bones[parent]
    return b

# --- ROOT + SPINE ---
bone("ROOT",    (0, BELLY,     SPINE_MID), (0, BELLY+0.03, SPINE_MID))
bone("Hips",    (0, BACK,      BACK_Z),    (0, BACK,       SPINE_MID), "ROOT")
bone("Spine",   (0, BACK,      SPINE_MID), (0, BACK,       FRONT_Z),   "Hips", True)
bone("Chest",   (0, BACK,      FRONT_Z),   (0, BACK+0.02,  FRONT_Z-D*0.1), "Spine", True)
bone("Neck",    (0, NECK_BASE, FRONT_Z-D*0.1),(0,NECK_BASE+H*0.08,FRONT_Z-D*0.17),"Chest",True)
bone("Head",    (0, NECK_BASE+H*0.08, FRONT_Z-D*0.17),
                (0, HEAD_CTR,         Z0+D*0.05), "Neck",True)
bone("Head_End",(0, HEAD_CTR, Z0+D*0.05),(0,HEAD_TOP, Z0), "Head",True)

# --- TAIL ---
bone("Tail_1",  (0, BACK,      Z1),        (0, BACK+H*0.08, Z1+D*0.05), "Hips")
bone("Tail_2",  (0, BACK+H*0.08,Z1+D*0.05),(0,BACK+H*0.15, Z1+D*0.09), "Tail_1",True)
bone("Tail_3",  (0, BACK+H*0.15,Z1+D*0.09),(0,BACK+H*0.22, Z1+D*0.12), "Tail_2",True)
bone("Tail_End",(0, BACK+H*0.22,Z1+D*0.12),(0,BACK+H*0.28, Z1+D*0.14), "Tail_3",True)

# --- FRONT LEGS ---
for side, sx in (("FrontL", -LEG_X), ("FrontR", LEG_X)):
    bone(f"{side}_Shoulder", (sx,BACK,     FRONT_Z),   (sx, HIP_Y,  FRONT_Z),   "Chest")
    bone(f"{side}_Upper",    (sx, HIP_Y,   FRONT_Z),   (sx, KNEE_Y, FRONT_Z+D*0.03), f"{side}_Shoulder",True)
    bone(f"{side}_Lower",    (sx, KNEE_Y,  FRONT_Z+D*0.03),(sx,PAW_Y,FRONT_Z),  f"{side}_Upper",True)
    bone(f"{side}_Paw",      (sx, PAW_Y,   FRONT_Z),   (sx, PAW_Y,  FRONT_Z-D*0.05),f"{side}_Lower",True)

# --- BACK LEGS ---
for side, sx in (("BackL", -LEG_X), ("BackR", LEG_X)):
    bone(f"{side}_Hip",   (sx, BACK,    BACK_Z),    (sx, HIP_Y,  BACK_Z),   "Hips")
    bone(f"{side}_Upper", (sx, HIP_Y,  BACK_Z),    (sx, KNEE_Y, BACK_Z-D*0.04),f"{side}_Hip",True)
    bone(f"{side}_Lower", (sx, KNEE_Y, BACK_Z-D*0.04),(sx,PAW_Y, BACK_Z),  f"{side}_Upper",True)
    bone(f"{side}_Paw",   (sx, PAW_Y,  BACK_Z),    (sx, PAW_Y,  BACK_Z+D*0.05),f"{side}_Lower",True)

bpy.ops.object.mode_set(mode='OBJECT')
print(f"Armature built: {len(arm.bones)} bones")

# ── 3. PARENT MESH → ARMATURE (Automatic Weights) ──────────────────────────────

bpy.ops.object.select_all(action='DESELECT')
mesh_obj.select_set(True)
arm_obj.select_set(True)
bpy.context.view_layer.objects.active = arm_obj
bpy.ops.object.parent_set(type='ARMATURE_AUTO')
print("Automatic weights applied.")

# ── 4. SIMPLE IDLE PREVIEW ANIMATION ──────────────────────────────────────────

bpy.context.scene.frame_start = 1
bpy.context.scene.frame_end   = 60   # 2 seconds at 30fps

bpy.context.view_layer.objects.active = arm_obj
arm_obj.select_set(True)
bpy.ops.object.mode_set(mode='POSE')
pose = arm_obj.pose

def set_key(bone_name, frame, loc=None, rot=None):
    pb = pose.bones.get(bone_name)
    if not pb: return
    if rot:
        pb.rotation_mode = 'XYZ'
        pb.rotation_euler = rot
        pb.keyframe_insert("rotation_euler", frame=frame)

BREATH = math.radians(1.5)   # chest rise
WAGS   = math.radians(25)    # tail swing

for f in [1, 30, 60]:
    sign = 1 if f == 30 else -1 if f == 60 else 0
    set_key("Chest",   f, rot=(BREATH * abs(math.sin((f-1)/60*math.pi)), 0, 0))
    set_key("Tail_1",  f, rot=(0, 0, WAGS  * sign))
    set_key("Tail_2",  f, rot=(0, 0, WAGS*0.6 * sign))
    set_key("Tail_3",  f, rot=(0, 0, WAGS*0.3 * sign))

bpy.ops.object.mode_set(mode='OBJECT')
print("Preview idle animation created (frames 1-60).")

# ── 5. RENAME ARMATURE ACTION ─────────────────────────────────────────────────

if arm_obj.animation_data and arm_obj.animation_data.action:
    arm_obj.animation_data.action.name = "Bella_Idle"

print("""
╔══════════════════════════════════════════════════════╗
║  Bella rigged!  Now export:                         ║
║  File → Export → FBX                               ║
║  ✅ Selected Objects  ✅ Armature  ✅ Mesh          ║
║  Save as:  bella_rigged.fbx                        ║
║  Then drag bella_rigged.fbx into Unity             ║
╚══════════════════════════════════════════════════════╝
""")
