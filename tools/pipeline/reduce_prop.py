#!/usr/bin/env python3
"""Reduce a generated 3D prop (Triple 3D / Tripo FBX, ~50k triangles, 4096 px textures) to game size.

Usage (needs the `bpy` module, Python 3.11):
    python reduce_prop.py <in.fbx> <out_dir> <name> [triangles=2500] [tex=512]

Writes <out_dir>/<name>.fbx (mesh only, no embedded texture) and <out_dir>/<name>_tex.jpg (base colour only).
`Art.Prop` in the game loads Props/<name> and Props/<name>_tex, scales to the size given in code and puts it on the floor.
"""
import sys, os, bpy
from PIL import Image


def main(src, out_dir, name, tris=2500, tex=512):
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.fbx(filepath=src)
    meshes = [o for o in bpy.data.objects if o.type == 'MESH']
    if not meshes:
        sys.exit("no mesh in " + src)
    bpy.ops.object.select_all(action='DESELECT')
    for o in meshes:
        o.select_set(True)
    bpy.context.view_layer.objects.active = meshes[0]
    if len(meshes) > 1:
        bpy.ops.object.join()
    ob = bpy.context.view_layer.objects.active
    bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)

    # base colour image = the one wired into Base Color, else the first non-normal/roughness/metallic one
    base = None
    mat = ob.material_slots[0].material if ob.material_slots else None
    if mat and mat.use_nodes:
        for n in mat.node_tree.nodes:
            if n.type == 'BSDF_PRINCIPLED':
                for l in n.inputs['Base Color'].links:
                    if l.from_node.type == 'TEX_IMAGE':
                        base = l.from_node.image
    if base is None:
        for im in bpy.data.images:
            if not any(k in im.name.lower() for k in ('normal', 'rough', 'metal')):
                base = im
                break

    before = len(ob.data.polygons)
    ratio = min(1.0, tris / float(before * 1.0 if before else 1))
    # triangulate first so the count is in triangles
    mod = ob.modifiers.new('tri', 'TRIANGULATE')
    bpy.ops.object.modifier_apply(modifier=mod.name)
    before = len(ob.data.polygons)
    mod = ob.modifiers.new('dec', 'DECIMATE')
    mod.decimate_type = 'COLLAPSE'
    mod.ratio = min(1.0, tris / float(before))
    mod.use_collapse_triangulate = True
    bpy.ops.object.modifier_apply(modifier=mod.name)
    after = len(ob.data.polygons)

    os.makedirs(out_dir, exist_ok=True)
    # drop the material's textures from the export: the game applies its own texture
    for s in ob.material_slots:
        if s.material:
            s.material.use_nodes = False
            s.material.name = name
    bpy.ops.object.select_all(action='DESELECT')
    ob.select_set(True)
    ob.name = "node_0"
    bpy.ops.export_scene.fbx(filepath=os.path.join(out_dir, name + ".fbx"), use_selection=True,
                             path_mode='STRIP', embed_textures=False, add_leaf_bones=False,
                             bake_anim=False, object_types={'MESH'})

    if base is not None:
        p = os.path.join(out_dir, "_tmp_base.png")
        base.filepath_raw = p
        base.file_format = 'PNG'
        # source may be packed/external: save a copy
        base.save()
        im = Image.open(p).convert("RGB").resize((tex, tex), Image.LANCZOS)
        im.save(os.path.join(out_dir, name + "_tex.jpg"), quality=82, optimize=True)
        os.remove(p)
    print("REDUCED %s: %d -> %d triangles, tex %s" % (name, before, after, "ok" if base is not None else "MISSING"))


if __name__ == "__main__":
    a = sys.argv[1:]
    if len(a) < 3:
        sys.exit(__doc__)
    main(a[0], a[1], a[2], int(a[3]) if len(a) > 3 else 2500, int(a[4]) if len(a) > 4 else 512)
