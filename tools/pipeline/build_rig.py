"""Bella asset pipeline, stage 1: clean + decimate + texture + armature + auto weights -> bella_rig.blend"""
import bpy, bmesh, numpy as np, mathutils, os, sys
SRC="../../art-source/bella_raw.glb"
OUT="out"; os.makedirs(OUT, exist_ok=True)
TARGET_TRIS=14000
bpy.ops.wm.read_factory_settings(use_empty=True)
bpy.ops.import_scene.gltf(filepath=SRC)
o=[x for x in bpy.context.scene.objects if x.type=='MESH'][0]
o.name="BellaMesh"; o.data.name="BellaMesh"
bpy.context.view_layer.objects.active=o; o.select_set(True)
bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)

# --- texture: 4096 -> 1024 base colour only
img=[i for i in bpy.data.images if i.colorspace_settings.name=='sRGB'][0]
img.scale(1024,1024)
img.filepath_raw=OUT+"/bella_base.png"; img.file_format='PNG'; img.save()
mat=o.data.materials[0]; mat.name="BellaMat"
nt=mat.node_tree
for n in list(nt.nodes):
    if n.type=='TEX_IMAGE' and n.image!=img: nt.nodes.remove(n)
    elif n.type=='NORMAL_MAP': nt.nodes.remove(n)
bsdf=[n for n in nt.nodes if n.type=='BSDF_PRINCIPLED'][0]
bsdf.inputs['Metallic'].default_value=0; bsdf.inputs['Roughness'].default_value=0.9

# --- weld + decimate
bm=bmesh.new(); bm.from_mesh(o.data)
bmesh.ops.remove_doubles(bm, verts=bm.verts, dist=1e-5)
bm.to_mesh(o.data); bm.free()
tris=len(o.data.polygons)
m=o.modifiers.new("dec",'DECIMATE'); m.ratio=TARGET_TRIS/tris; m.use_collapse_triangulate=True
bpy.ops.object.modifier_apply(modifier="dec")
bpy.ops.object.shade_smooth()
print("TRIS", len(o.data.polygons), "VERTS", len(o.data.vertices))

# --- armature
arm=bpy.data.armatures.new("Armature"); ao=bpy.data.objects.new("Armature", arm)
bpy.context.scene.collection.objects.link(ao)
bpy.context.view_layer.objects.active=ao
bpy.ops.object.mode_set(mode='EDIT')
FX,BX=0.106,0.124
B=[ # name, head, tail, parent, deform, connect
 ("Root",(0,0.08,0),(0,0.08,0.10),None,False),
 ("Hips",(0,0.33,0.30),(0,0.12,0.31),"Root",True),
 ("Spine",(0,0.12,0.31),(0,-0.05,0.32),"Hips",True),
 ("Chest",(0,-0.05,0.32),(0,-0.14,0.40),"Spine",True),
 ("Neck",(0,-0.14,0.40),(0,-0.15,0.52),"Chest",True),
 ("Head",(0,-0.15,0.52),(0,-0.17,0.85),"Neck",True),
 ("Tail_1",(0,0.40,0.36),(0,0.50,0.36),"Hips",True),
 ("Tail_2",(0,0.50,0.36),(0,0.58,0.40),"Tail_1",True),
 ("Tail_3",(0,0.58,0.40),(0,0.65,0.45),"Tail_2",True),
]
for s,sx in (("L",1),("R",-1)):
    B+=[
     (f"Ear_{s}",(sx*0.22,-0.12,0.74),(sx*0.29,-0.10,0.50),"Head",True),
     (f"Front{s}_Upper",(sx*FX,-0.15,0.30),(sx*FX,-0.16,0.15),"Chest",True),
     (f"Front{s}_Lower",(sx*FX,-0.16,0.15),(sx*FX,-0.165,0.05),f"Front{s}_Upper",True),
     (f"Front{s}_Paw",(sx*FX,-0.165,0.05),(sx*FX,-0.24,0.02),f"Front{s}_Lower",True),
     (f"Back{s}_Upper",(sx*BX,0.30,0.30),(sx*BX,0.31,0.15),"Hips",True),
     (f"Back{s}_Lower",(sx*BX,0.31,0.15),(sx*BX,0.30,0.05),f"Back{s}_Upper",True),
     (f"Back{s}_Paw",(sx*BX,0.30,0.05),(sx*BX,0.235,0.02),f"Back{s}_Lower",True),
    ]
for name,h,t,p,d in B:
    eb=arm.edit_bones.new(name); eb.head=h; eb.tail=t; eb.use_deform=d; eb.roll=0
    if p: eb.parent=arm.edit_bones[p]
bpy.ops.object.mode_set(mode='OBJECT')

# --- auto weights
bpy.ops.object.select_all(action='DESELECT')
o.select_set(True); ao.select_set(True); bpy.context.view_layer.objects.active=ao
bpy.ops.object.parent_set(type='ARMATURE_AUTO')
unw=sum(1 for v in o.data.vertices if len(v.groups)==0)
print("UNWEIGHTED", unw, "GROUPS", len(o.vertex_groups))
bpy.context.view_layer.objects.active=o
bpy.ops.object.vertex_group_limit_total(group_select_mode='ALL', limit=4)
bpy.ops.object.vertex_group_normalize_all(group_select_mode='ALL', lock_active=False)
bpy.ops.wm.save_as_mainfile(filepath=OUT+"/bella_rig.blend")
