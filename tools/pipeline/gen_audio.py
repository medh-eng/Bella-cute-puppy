"""All of Bella's sounds, synthesised from scratch (no samples, no third-party audio -> no licences to track)."""
import numpy as np, wave
SR=22050; OUT="audio"
def t(d): return np.arange(int(SR*d))/SR
def env(n,a=0.005,r=0.08):
    e=np.ones(n); na=int(SR*a); nr=int(SR*r)
    e[:na]=np.linspace(0,1,na); e[-nr:]*=np.linspace(1,0,nr); return e
def save(name,x,vol=0.8):
    x=x/ (np.abs(x).max()+1e-9)*vol
    w=wave.open(f"{OUT}/{name}.wav","wb"); w.setnchannels(1); w.setsampwidth(2); w.setframerate(SR)
    w.writeframes((x*32767).astype(np.int16).tobytes()); w.close()
def tone(f,d,dec=6.0,harm=(1,0.35,0.12)):
    tt=t(d); x=sum(a*np.sin(2*np.pi*f*(i+1)*tt) for i,a in enumerate(harm)); return x*np.exp(-dec*tt)*env(len(tt),0.004,0.03)
def seq(notes,gap=0.0):
    L=int(SR*(max(s+d for s,f,d in notes)+0.3)); x=np.zeros(L)
    for s,f,d in notes:
        y=tone(f,d); i=int(SR*s); x[i:i+len(y)]+=y
    return x
N={n:440*2**((i-9)/12) for i,n in enumerate("C C# D D# E F F# G G# A A# B".split())}
def n(name,o=4): return N[name]*2**(o-4)
rng=np.random.default_rng(7)
# ui
save("tap",tone(880,0.07,30),0.5)
tt=t(0.14); save("pop",np.sin(2*np.pi*(300+2600*tt/0.14)*tt)*np.exp(-22*tt)*env(len(tt)),0.7)
save("chime",seq([(0,n("E",5),0.5),(0.09,n("G",5),0.5),(0.18,n("C",6),0.8)]),0.6)
save("coin",seq([(0,n("B",5),0.12),(0.07,n("E",6),0.5)]),0.55)
save("success",seq([(0,n("C",5),0.2),(0.12,n("E",5),0.2),(0.24,n("G",5),0.2),(0.36,n("C",6),0.9),(0.36,n("E",6),0.9)]),0.65)
save("gift",seq([(i*0.07,n(x,o),0.5) for i,(x,o) in enumerate([("C",5),("D",5),("E",5),("G",5),("A",5),("C",6),("E",6)])]),0.6)
tt=t(0.3); save("whoosh",rng.standard_normal(len(tt))*np.sin(np.pi*tt/0.3)**2*np.linspace(0.3,1,len(tt)),0.3)
save("oops",seq([(0,n("E",4),0.25),(0.16,n("C",4),0.5)]),0.45)
# puppy voice
def yip(f0=820,f1=520,d=0.13):
    tt=t(d); f=f0+(f1-f0)*(tt/d)**0.7; ph=2*np.pi*np.cumsum(f)/SR
    x=sum(np.sin(k*ph)/k**1.2 for k in range(1,9)); x+=0.25*rng.standard_normal(len(tt))
    return np.tanh(1.6*x)*np.sin(np.pi*(tt/d)**0.6)
def join(parts,gaps):
    out=[]
    for p,g in zip(parts,gaps): out+= [p,np.zeros(int(SR*g))]
    return np.concatenate(out)
save("bark",join([yip(),yip(880,560,0.15)],[0.09,0.05]),0.7)
save("yip",join([yip(980,700,0.10)],[0.03]),0.6)
tt=t(0.5); f=520+140*np.sin(2*np.pi*3.2*tt)-180*tt; ph=2*np.pi*np.cumsum(f)/SR
save("whine",(np.sin(ph)+0.4*np.sin(2*ph)+0.2*np.sin(3*ph))*np.sin(np.pi*tt/0.5),0.4)
def crunch():
    tt=t(0.07); return rng.standard_normal(len(tt))*np.exp(-45*tt)*(1+np.sin(2*np.pi*90*tt))
save("munch",join([crunch() for _ in range(5)],[0.11,0.09,0.12,0.1,0.05]),0.55)
def blip(f): tt=t(0.09); return np.sin(2*np.pi*(f+f*1.5*tt/0.09)*tt)*np.sin(np.pi*tt/0.09)
save("bubbles",join([blip(f) for f in (500,760,620,900,700,1040)],[0.05,0.03,0.06,0.03,0.04,0.05]),0.5)
tt=t(0.9); nz=rng.standard_normal(len(tt)); k=np.exp(-np.arange(60)/14.0); nz=np.convolve(nz,k/k.sum(),mode="same")
save("splash",nz*np.exp(-3.5*tt)*env(len(tt),0.01,0.2),0.5)
tt=t(1.6); nz=rng.standard_normal(len(tt)); k=np.ones(90)/90; nz=np.convolve(nz,k,mode="same")
save("snore",nz*(np.sin(np.pi*tt/1.6)**2)*(1+0.5*np.sin(2*np.pi*24*tt)),0.35)
tt=t(0.35); nz=rng.standard_normal(len(tt))*np.exp(-12*tt); save("sneeze",np.concatenate([yip(600,900,0.12)*0.5,np.zeros(int(SR*0.04)),nz]),0.5)
save("sparkle",seq([(i*0.05,n(x,6),0.35) for i,x in enumerate(["C","E","G","B","G","E"])])+0,0.4)
# music: two original loops, music-box timbre
def music(mel,bass,beat,name,vol):
    L=int(SR*beat*sum(d for _,d in mel)); x=np.zeros(L+SR)
    pos=0.0
    for f,d in mel:
        if f: y=tone(f,min(1.6,d*beat*2.2),3.0,(1,0.25,0.08)); i=int(SR*pos*beat); x[i:i+len(y)]+=y
        pos+=d
    pos=0.0
    for f,d in bass:
        if f: y=tone(f,min(2.2,d*beat*1.5),1.8,(1,0.15))*0.55; i=int(SR*pos*beat); x[i:i+len(y)]+=y
        pos+=d
    x[:SR]+=x[L:L+SR]; save(name,x[:L],vol)   # wrap the tail so the loop is seamless
R=None
day=[("C",5,1),("E",5,1),("G",5,1),("E",5,1),("A",5,1),("G",5,1),("E",5,2), ("D",5,1),("F",5,1),("A",5,1),("F",5,1),("G",5,1),("E",5,1),("D",5,2),
     ("C",5,1),("E",5,1),("G",5,1),("C",6,1),("B",5,1),("A",5,1),("G",5,2), ("F",5,1),("A",5,1),("G",5,1),("E",5,1),("D",5,1),("G",5,1),("C",5,2)]
dayb=[("C",3,4),("A",2,4),("F",2,4),("G",2,4),("C",3,4),("E",3,4),("F",2,4),("G",2,2),("C",3,2)]
music([(n(a,o),d) for a,o,d in day],[(n(a,o),d) for a,o,d in dayb],0.30,"music_day",0.5)
night=[("E",5,2),("G",5,1),("E",5,3),("D",5,2),("F",5,1),("D",5,3),("C",5,2),("E",5,1),("G",5,2),("A",5,1),("G",5,3),(None,0,3),
       ("A",5,2),("G",5,1),("E",5,3),("F",5,2),("E",5,1),("D",5,3),("C",5,2),("D",5,1),("E",5,2),("D",5,1),("C",5,3),(None,0,3)]
nightb=[("C",3,6),("G",2,6),("A",2,6),("E",2,6),("F",2,6),("G",2,6),("A",2,6),("C",3,6)]
music([((n(a,o) if a else None),d) for a,o,d in night],[(n(a,o),d) for a,o,d in nightb],0.42,"music_night",0.45)
import os; print({f:os.path.getsize(OUT+"/"+f)//1024 for f in sorted(os.listdir(OUT))})
