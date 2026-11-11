"""Paint Bella's expression variants straight into her UV texture, using the baked texel->3D position map.
Shapes are defined in FRONT VIEW coordinates (x = side, z = height, Blender units)."""
import numpy as np
from PIL import Image
OUT="out"
d=np.load(OUT+"/maps.npz"); pos,nor,mask=d["pos"],d["nor"],d["mask"]
base=np.array(Image.open(OUT+"/bella_base.png").convert("RGB")).astype(np.float32)[::-1]
X=pos[...,0]; Y=pos[...,1]; Z=pos[...,2]
front=mask&(Y<-0.18)&(Z>0.5)
facing=front&(nor[...,1]<-0.05)
AA=0.0018
def sstep(d,w=AA): return np.clip(0.5-d/w,0,1)          # d<0 inside
def ellipse(cx,cz,rx,rz): return (np.sqrt(((X-cx)/rx)**2+((Z-cz)/rz)**2)-1)*min(rx,rz)
def paint(img,alpha,color,region=None):
    a=(alpha*(region if region is not None else facing))[...,None]
    return img*(1-a)+np.array(color,np.float32)*a
def sample(x,z,r=0.012):
    m=facing&(np.hypot(X-x,Z-z)<r); return base[m].mean(0)
EYES=[(-0.099,0.692),(0.099,0.694)]
ERX,ERZ=0.056,0.062
SKIN=[sample(sx*0.10,0.775) for sx in (-1,1)]
SKIN_LOW=[sample(sx*0.13,0.615) for sx in (-1,1)]
MUZ=(sample(-0.105,0.59,0.012)+sample(0.105,0.59,0.012))/2
DARK=(72,40,24); LIDLINE=(96,56,34)
def skin_grad(i):
    cx,cz=EYES[i]; t=np.clip((Z-(cz-ERZ))/(2*ERZ),0,1)[...,None]
    return SKIN_LOW[i]*(1-t)+SKIN[i]*t
def cover_eye(img,i,above=None):
    """paint lid colour over the eye; `above` = function z_line(x): only cover above that line"""
    cx,cz=EYES[i]; a=sstep(ellipse(cx,cz,ERX,ERZ),0.006)
    if above is not None: a=a*sstep(above(X)-Z)
    g=skin_grad(i); m=(a*facing)[...,None]
    return img*(1-m)+g*m
def stroke(img,dist,width,color,region=None):
    return paint(img,sstep(np.abs(dist)-width/2),color,region)
def arc(img,cx,cz,w,h,width,color,up=True):
    """parabolic arc spanning cx±w, bulging up (∩) or down (∪) by h"""
    u=np.clip((X-cx)/w,-1,1); zc=cz+(h*(1-u*u) if up else -h*(1-u*u))
    dist=np.where(np.abs((X-cx)/w)<=1, Z-zc, 1.0)
    return stroke(img,dist,width,color)
def close_mouth(img,curve):
    a=sstep(ellipse(0.004,0.566,0.094,0.050),0.022)
    region=mask&(Y<-0.15)&(Z>0.49)&(Z<0.64)
    img=paint(img,a,MUZ,region)
    if curve=="smile": img=arc(img,0.004,0.585,0.045,0.014,0.0065,DARK,up=False)
    elif curve=="frown": img=arc(img,0.004,0.566,0.034,0.011,0.0065,DARK,up=True)
    elif curve=="o":
        img=paint(img,sstep(ellipse(0.004,0.572,0.014,0.016)),DARK,region)
    return img
def star(cx,cz,R,r=0.45,n=5):
    ang=np.arctan2(Z-cz,X-cx)+np.pi/2; rad=np.hypot(X-cx,Z-cz)
    seg=2*np.pi/n; a=np.abs(((ang+seg/2)%seg)-seg/2)
    # star edge radius at angle a (between tip at 0 and valley at seg/2)
    p1=np.array([0,R]); p2=np.array([R*r*np.sin(seg/2),R*r*np.cos(seg/2)])
    px=rad*np.sin(a); py=rad*np.cos(a)
    e=p2-p1; nrm=np.array([e[1],-e[0]])/np.hypot(*e)
    return -((px-p1[0])*nrm[0]+(py-p1[1])*nrm[1])
def save(img,name):
    Image.fromarray(np.clip(img,0,255).astype(np.uint8)[::-1]).save(f"{OUT}/bella_face_{name}.png",optimize=True)
    return img
V={}
V["happy"]=base.copy()
# closed happy eyes  ^ ^   (also used for blinking)
img=base.copy()
for i,(cx,cz) in enumerate(EYES):
    img=cover_eye(img,i); img=arc(img,cx,cz-0.012,0.040,0.022,0.0085,DARK,up=True)
V["blink"]=img
# sleepy: heavy lids, closed soft smile
img=base.copy()
for i,(cx,cz) in enumerate(EYES):
    line=lambda x,cz=cz: cz-0.004+0*x
    img=cover_eye(img,i,above=line)
    inside=sstep(ellipse(cx,cz,ERX*0.93,ERZ*0.93))
    img=paint(img,sstep(np.abs(Z-(cz-0.004))-0.004)*inside,LIDLINE)
V["sleepy"]=close_mouth(img,"smile")
# sad: drooping outer lids + tiny frown
img=base.copy()
for i,(cx,cz) in enumerate(EYES):
    sg=-1 if cx<0 else 1
    line=lambda x,cx=cx,cz=cz,sg=sg: cz+0.030-0.55*sg*(x-cx)
    img=cover_eye(img,i,above=line)
    inside=sstep(ellipse(cx,cz,ERX*0.93,ERZ*0.93))
    img=paint(img,sstep(np.abs(Z-line(X))-0.0035)*inside,LIDLINE)
V["sad"]=close_mouth(img,"frown")
# begging: huge wet sparkles + little 'o' mouth
img=base.copy()
for i,(cx,cz) in enumerate(EYES):
    img=paint(img,sstep(ellipse(cx-0.014,cz+0.020,0.017,0.019)),(255,255,255))
    img=paint(img,sstep(ellipse(cx+0.018,cz-0.016,0.010,0.011)),(255,255,255))
    img=paint(img,sstep(ellipse(cx,cz-0.036,0.026,0.010))*0.55,(170,215,255))
    sg=-1 if cx<0 else 1
    line=lambda x,cx=cx,cz=cz,sg=sg: cz+0.044-0.30*sg*(x-cx)
    img=cover_eye(img,i,above=line)
V["begging"]=close_mouth(img,"o")
# excited: star eyes
img=base.copy()
for i,(cx,cz) in enumerate(EYES):
    img=paint(img,sstep(star(cx,cz,0.036),0.002),(120,70,20))
    img=paint(img,sstep(star(cx,cz,0.030),0.002),(255,214,64))
    img=paint(img,sstep(ellipse(cx-0.008,cz+0.010,0.006,0.006)),(255,250,220))
V["excited"]=img
for k,v in V.items(): save(v,k)
# ---- front-view contact sheet for checking
S=420; X0,X1,Z0,Z1=-0.36,0.36,0.44,0.96; H=int(S*(Z1-Z0)/(X1-X0))
sel=mask&(nor[...,1]<-0.25)&(Z>Z0)&(Y<-0.05)
order=np.argsort(-Y[sel]); px=((X[sel]-X0)/(X1-X0)*S).astype(int)[order]; pz=((Z1-Z[sel])/(X1-X0)*S).astype(int)[order]
sheet=Image.new("RGB",(S*6,H),"white")
for n,k in enumerate(["happy","excited","sad","sleepy","begging","blink"]):
    c=np.clip(V[k],0,255).astype(np.uint8)[sel][order]; im=np.full((H,S,3),255,np.uint8)
    for dx in (0,1):
        for dz in (0,1):
            ok=(px+dx<S)&(pz+dz<H); im[pz[ok]+dz,px[ok]+dx]=c[ok]
    sheet.paste(Image.fromarray(im),(S*n,0))
sheet.save("./faces_check.png")
