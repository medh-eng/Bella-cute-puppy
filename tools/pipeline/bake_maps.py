import bpy, numpy as np
bpy.ops.wm.open_mainfile(filepath="out/bella_rig.blend")
me=bpy.data.objects["BellaMesh"].data
me.calc_loop_triangles()
N=1024
uv=np.array([l.uv[:] for l in me.uv_layers[0].data])
co=np.array([v.co[:] for v in me.vertices]); no=np.array([v.normal[:] for v in me.vertices])
pos=np.zeros((N,N,3),np.float32); nor=np.zeros((N,N,3),np.float32); mask=np.zeros((N,N),bool)
for t in me.loop_triangles:
    ls=t.loops; vs=t.vertices
    p=uv[list(ls)]*N; 
    x0,y0=np.floor(p.min(0)-1).astype(int); x1,y1=np.ceil(p.max(0)+1).astype(int)
    x0=max(x0,0);y0=max(y0,0);x1=min(x1,N-1);y1=min(y1,N-1)
    if x1<x0 or y1<y0: continue
    xs,ys=np.meshgrid(np.arange(x0,x1+1)+0.5,np.arange(y0,y1+1)+0.5)
    a,b,c=p
    den=(b[1]-c[1])*(a[0]-c[0])+(c[0]-b[0])*(a[1]-c[1])
    if abs(den)<1e-12: continue
    w0=((b[1]-c[1])*(xs-c[0])+(c[0]-b[0])*(ys-c[1]))/den
    w1=((c[1]-a[1])*(xs-c[0])+(a[0]-c[0])*(ys-c[1]))/den
    w2=1-w0-w1
    eps=-0.02*0+(-0.6/ max(1.0,np.sqrt(abs(den))))  # ~0.6px dilation for seams
    inside=(w0>=eps)&(w1>=eps)&(w2>=eps)
    if not inside.any(): continue
    W=np.stack([w0,w1,w2],-1)[inside]
    yy=(ys[inside]-0.5).astype(int); xx=(xs[inside]-0.5).astype(int)
    pos[yy,xx]=W@co[list(vs)]; nor[yy,xx]=W@no[list(vs)]; mask[yy,xx]=True
np.savez_compressed("out/maps.npz",pos=pos,nor=nor,mask=mask)
print("covered",mask.mean())
