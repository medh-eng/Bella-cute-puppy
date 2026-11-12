"""Stage 2: bake clips onto one timeline -> FBX + clips.json; optional preview renders."""
import bpy, sys, os, json, math, mathutils, importlib.util
HERE="."
spec=importlib.util.spec_from_file_location("anims", HERE+"/anims.py"); A=importlib.util.module_from_spec(spec); spec.loader.exec_module(A)
OUT=HERE+"/out"
args=sys.argv[sys.argv.index("--")+1:] if "--" in sys.argv else []
PREVIEW=[a for a in args if not a.startswith("-")]
bpy.ops.wm.open_mainfile(filepath=OUT+"/bella_rig.blend")
ao=bpy.data.objects["Armature"]; mesh=bpy.data.objects["BellaMesh"]
sc=bpy.context.scene; sc.render.fps=A.FPS
for pb in ao.pose.bones: pb.rotation_mode='QUATERNION'
REST={pb.name: pb.bone.matrix_local.to_3x3() for pb in ao.pose.bones}
def E(rx,ry,rz):
    return (mathutils.Matrix.Rotation(math.radians(rz),3,'Z') @ mathutils.Matrix.Rotation(math.radians(ry),3,'Y') @ mathutils.Matrix.Rotation(math.radians(rx),3,'X'))
def apply(pose):
    for pb in ao.pose.bones:
        pb.rotation_quaternion=(1,0,0,0); pb.location=(0,0,0)
    for name,v in pose.items():
        pb=ao.pose.bones[name]; M=REST[name]
        if isinstance(v,dict):
            R=E(*v.get("rot",(0,0,0))); loc=mathutils.Vector(v.get("loc",(0,0,0)))
            # optional pivot (world point the rotation happens about), default = bone head
            head=pb.bone.matrix_local.to_translation()
            piv=mathutils.Vector((0, v.get("pivot_y",head.y), v.get("pivot_z",head.z)))
            loc=loc+(piv-head)-R@(piv-head)
            pb.location=M.inverted()@loc
        else: R=E(*v)
        pb.rotation_quaternion=(M.inverted()@R@M).to_quaternion()
import numpy as np
SINK=0.006   # how far the lowest point may rest below the floor (reads as "weight" with the blob shadow)
def ground():
    """Floor contact: lift the whole puppy (Root bone) until nothing is under the floor.
    Poses are authored as rotations about pivots, so sits, bows, dances and rolls would
    otherwise push paws, tummy or head through the ground. Never pulls an airborne pose down."""
    root=ao.pose.bones["Root"]
    for _ in range(3):
        bpy.context.view_layer.update()
        dg=bpy.context.evaluated_depsgraph_get(); ob=mesh.evaluated_get(dg); me=ob.to_mesh()
        co=np.empty(len(me.vertices)*3); me.vertices.foreach_get("co",co); co=co.reshape(-1,3)
        M=np.array(ob.matrix_world); zmin=float((co@M[:3,:3].T+M[:3,3])[:,2].min()); ob.to_mesh_clear()
        lift=-zmin-SINK
        if lift<=2e-4: break
        root.location=root.location+REST["Root"].inverted()@mathutils.Vector((0,0,lift))
L=A.layout()
def key_all(f):
    for pb in ao.pose.bones:
        pb.keyframe_insert("rotation_quaternion",frame=f)
        if pb.name=="Root": pb.keyframe_insert("location",frame=f)

if PREVIEW:
    sc.render.engine='CYCLES'; sc.cycles.samples=6; sc.cycles.device='CPU'; sc.cycles.use_denoising=False
    sc.render.resolution_x=220; sc.render.resolution_y=220
    w=bpy.data.worlds.new("w"); sc.world=w; w.use_nodes=True
    w.node_tree.nodes["Background"].inputs[0].default_value=(1,1,1,1)
    bpy.ops.mesh.primitive_plane_add(size=6); fl=bpy.context.object
    fm=bpy.data.materials.new("fl"); fm.diffuse_color=(0.55,0.75,0.6,1); fm.use_nodes=True
    fm.node_tree.nodes["Principled BSDF"].inputs[0].default_value=(0.45,0.7,0.55,1); fl.data.materials.append(fm)
    cam=bpy.data.objects.new("cam",bpy.data.cameras.new("cam")); sc.collection.objects.link(cam); sc.camera=cam
    cam.data.lens=60
    c=mathutils.Vector((0,0.1,0.42))
    views=[(2.6,-0.6,0.5),(1.6,-2.4,0.9)]
    from PIL import Image
    for n,s,e,loop,fn in L:
        if "all" not in PREVIEW and n not in PREVIEW: continue
        ts=[0,0.2,0.4,0.6,0.8] if n not in ("Sit","Sleep","Beg","Eat","PlayBow","Paw","Idle") else [0.25]
        if os.environ.get("PREVIEW_TS"): ts=[float(x) for x in os.environ["PREVIEW_TS"].split(",")]
        sheet=Image.new("RGB",(220*len(ts),220*len(views)),"white")
        for i,t in enumerate(ts):
            apply(fn(t)); ground(); bpy.context.view_layer.update()
            for j,vw in enumerate(views):
                cam.location=c+mathutils.Vector(vw); d=c-cam.location
                cam.rotation_euler=d.to_track_quat('-Z','Y').to_euler()
                sc.render.filepath=f"/tmp/prev.png"; bpy.ops.render.render(write_still=True)
                sheet.paste(Image.open("/tmp/prev.png").convert("RGB"),(220*i,220*j))
        sheet.save(f"{HERE}/prev_{n}.png"); print("PREVIEW",n)
    sys.exit(0)

for n,s,e,loop,fn in L:
    fr=e-s
    for i in range(fr+1):
        apply(fn(i/fr if not loop else (i%fr)/fr if i<fr else 0.0)); ground(); key_all(s+i)
    # hold pose through the gap so sampling just past the end is safe
    for g in range(1,A.GAP): key_all(e+g)
sc.frame_start=0; sc.frame_end=L[-1][2]+A.GAP-1
act=ao.animation_data.action; act.name="BellaAll"
for pb in ao.pose.bones: pb.rotation_quaternion=(1,0,0,0); pb.location=(0,0,0)
sc.frame_set(0)
json.dump({"fps":A.FPS,"clips":[{"name":n,"start":s,"end":e,"loop":loop} for n,s,e,loop,fn in L]}, open(OUT+"/clips.json","w"), indent=1)
bpy.ops.object.select_all(action='DESELECT'); ao.select_set(True); mesh.select_set(True)
bpy.ops.export_scene.fbx(filepath=OUT+"/bella.fbx", use_selection=True, object_types={'ARMATURE','MESH'},
    add_leaf_bones=False, bake_anim=True, bake_anim_use_all_bones=True, bake_anim_use_nla_strips=False,
    bake_anim_use_all_actions=False, bake_anim_force_startend_keying=True, bake_anim_step=1.0, bake_anim_simplify_factor=0.0,
    path_mode='STRIP', embed_textures=False, mesh_smooth_type='FACE', use_armature_deform_only=False,
    apply_scale_options='FBX_SCALE_UNITS', axis_forward='-Z', axis_up='Y')
bpy.ops.wm.save_as_mainfile(filepath=OUT+"/bella_anim.blend")
print("EXPORTED frames", sc.frame_end)
