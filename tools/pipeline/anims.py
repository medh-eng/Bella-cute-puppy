"""Bella clip definitions. Angles in degrees about REST-WORLD axes (Blender: X = side axis, -Y = forward, Z = up).
 +rx on Head = nod down; +rx on a leg = swing backward; +rx on tail = lift; rz = yaw; ry = roll."""
from math import sin, cos, pi, radians
FPS=30
def S(x): return sin(2*pi*x)
def C(x): return cos(2*pi*x)
def ease(x):
    x=max(0.0,min(1.0,x)); return x*x*(3-2*x)
def bump(x,a,b):
    """0 -> 1 -> 0 between a and b"""
    if x<=a or x>=b: return 0.0
    return sin(pi*(x-a)/(b-a))
LEGS=["FrontL","FrontR","BackL","BackR"]
def breathe(p,t,amt=1.0):
    p["Spine"]=(1.2*amt*S(t),0,0); p["Chest"]=(-1.0*amt*S(t),0,0)

def idle(t):
    p={}; breathe(p,t)
    p["Head"]=(1.5*S(t-0.1),2.0*S(t),0); p["Neck"]=(1.0*S(t),0,0)
    p["Root"]=dict(loc=(0,0,0.004*S(t)))
    return p

def walk(t):
    p={}
    for leg,ph in (("FrontL",0.0),("BackR",0.05),("FrontR",0.5),("BackL",0.55)):
        sw=26*S(t+ph); lift=max(0.0,-C(t+ph))
        p[leg+"_Upper"]=(sw-8*lift,0,0); p[leg+"_Lower"]=(26*lift,0,0); p[leg+"_Paw"]=(10*lift,0,0)
    p["Root"]=dict(loc=(0,0,0.010*C(2*t)), rot=(0,2.0*S(t),0))
    p["Hips"]=(0,0,4*S(t)); p["Chest"]=(0,0,-5*S(t)); p["Neck"]=(2*S(2*t),0,2*S(t)); p["Head"]=(3*S(2*t+0.1),0,0)
    p["Tail_1"]=(8,0,0)
    return p

def run(t):
    p={}
    for leg,ph in (("FrontL",0.0),("FrontR",0.08),("BackL",0.45),("BackR",0.53)):
        sw=42*S(t+ph); lift=max(0.0,-C(t+ph))
        p[leg+"_Upper"]=(sw-10*lift,0,0); p[leg+"_Lower"]=(38*lift,0,0); p[leg+"_Paw"]=(15*lift,0,0)
    hop=max(0.0,S(t+0.15))
    p["Root"]=dict(loc=(0,0,0.05*hop), rot=(7*S(t+0.3),0,0))
    p["Spine"]=(6*S(t+0.5),0,0); p["Neck"]=(-6*S(t+0.3),0,0); p["Head"]=(-4*S(t+0.3),0,0)
    p["Tail_1"]=(20+10*S(t),0,0); p["Ear_L"]=(0,-12*hop,0); p["Ear_R"]=(0,12*hop,0)
    return p

def sit_pose(k=1.0, t=0.0):
    """k = 0 standing .. 1 seated"""
    p={}
    p["Root"]=dict(loc=(0,0.02*k,-0.015*k), rot=(-30*k,0,0))
    for s in ("L","R"):
        sg=1 if s=="L" else -1
        p["Front"+s+"_Upper"]=(26*k,0,0); p["Front"+s+"_Lower"]=(4*k,0,0); p["Front"+s+"_Paw"]=(0,0,0)
        p["Back"+s+"_Upper"]=(-48*k,0,sg*14*k); p["Back"+s+"_Lower"]=(62*k,0,0); p["Back"+s+"_Paw"]=(16*k,0,0)
    p["Spine"]=(6*k+1.2*S(t),0,0); p["Chest"]=(4*k-1.0*S(t),0,0); p["Neck"]=(10*k,0,0); p["Head"]=(10*k+1.5*S(t),2*S(t)*k,0)
    p["Tail_1"]=(18*k,0,0); p["Tail_2"]=(10*k,0,0)
    return p
def sit(t): return sit_pose(1.0,t)

def sleep(t):
    p={}; b=S(t)
    p["Root"]=dict(loc=(0,0,-0.13+0.004*b), rot=(0,0,0))
    for s in ("L","R"):
        sg=1 if s=="L" else -1
        p["Front"+s+"_Upper"]=(-74,0,sg*8); p["Front"+s+"_Lower"]=(-8,0,0); p["Front"+s+"_Paw"]=(-4,0,0)
        p["Back"+s+"_Upper"]=(-70,0,sg*28); p["Back"+s+"_Lower"]=(80,0,0); p["Back"+s+"_Paw"]=(20,0,0)
    p["Spine"]=(2+1.5*b,0,0); p["Chest"]=(4-1.2*b,0,0); p["Neck"]=(22,0,6); p["Head"]=(16+1.0*b,8,8)
    p["Tail_1"]=(-10,0,35); p["Tail_2"]=(-6,0,30); p["Tail_3"]=(0,0,25)
    p["Ear_L"]=(0,6,0); p["Ear_R"]=(0,-6,0)
    return p

def eat(t):
    p={}; ch=S(3*t)
    p["Root"]=dict(loc=(0,0,-0.01), rot=(5,0,0))
    p["Chest"]=(10,0,0); p["Neck"]=(34+4*ch,0,0); p["Head"]=(30+7*ch,0,3*S(t))
    for s in ("L","R"):
        p["Front"+s+"_Upper"]=(-10,0,0); p["Front"+s+"_Lower"]=(8,0,0)
    p["Tail_1"]=(22,0,0)
    return p

def happy(t):
    """bouncy little hops on the spot"""
    p={}; h=max(0.0,S(t)); sq=max(0.0,-S(t))
    p["Root"]=dict(loc=(0,0,0.07*h-0.012*sq), rot=(-6*h,0,0))
    for s in ("L","R"):
        p["Front"+s+"_Upper"]=(-22*h+10*sq,0,0); p["Front"+s+"_Lower"]=(28*h,0,0)
        p["Back"+s+"_Upper"]=(14*h-10*sq,0,0); p["Back"+s+"_Lower"]=(10*h+12*sq,0,0)
    p["Neck"]=(-8*h,0,0); p["Head"]=(-8*h+4*sq,0,0)
    p["Ear_L"]=(0,-18*h,0); p["Ear_R"]=(0,18*h,0); p["Tail_1"]=(25,0,0)
    return p

def rollover(t):
    """one-shot: flop down, roll 360 about the body axis, pop back up"""
    p={}
    down=ease(t/0.2)-ease((t-0.8)/0.2)
    roll=360*ease((t-0.18)/0.64)
    p["Root"]=dict(loc=(0,0,-0.10*down+0.16*bump(t,0.2,0.8)*0+0.0), rot=(0,roll,0), pivot_z=0.20)
    for s in ("L","R"):
        p["Front"+s+"_Upper"]=(-55*down,0,0); p["Front"+s+"_Lower"]=(50*down,0,0)
        p["Back"+s+"_Upper"]=(-45*down,0,0); p["Back"+s+"_Lower"]=(70*down,0,0)
    p["Neck"]=(10*down,0,0); p["Head"]=(-10*down,0,0)
    return p

def dance(t):
    """up on hind legs, front paws pedalling, hips wiggling"""
    p={}; k=1.0
    p["Root"]=dict(loc=(0,-0.06,0.015*abs(S(2*t))), rot=(-62,0,0), pivot_y=0.30)
    p["Hips"]=(0,0,8*S(2*t)); p["Spine"]=(10,0,-6*S(2*t)); p["Chest"]=(8,0,0)
    p["Neck"]=(22,0,6*S(2*t)); p["Head"]=(24,8*S(2*t),0)
    for s,ph in (("L",0.0),("R",0.5)):
        sg=1 if s=="L" else -1
        p["Front"+s+"_Upper"]=(-40+22*S(2*t+ph),0,0); p["Front"+s+"_Lower"]=(35,0,0); p["Front"+s+"_Paw"]=(25,0,0)
        p["Back"+s+"_Upper"]=(52+5*S(2*t+ph),0,sg*6); p["Back"+s+"_Lower"]=(8,0,0); p["Back"+s+"_Paw"]=(0,0,0)
    p["Tail_1"]=(30,0,25*S(4*t)); p["Ear_L"]=(0,-10*abs(S(2*t)),0); p["Ear_R"]=(0,10*abs(S(2*t)),0)
    return p

def shake(t):
    """bath shake-off: fast twist travelling down the body"""
    p={}; env=bump(t,0.0,1.0); f=5
    p["Head"]=(0,22*env*S(f*t),0); p["Neck"]=(0,10*env*S(f*t-0.1),0)
    p["Chest"]=(0,10*env*S(f*t-0.2),0); p["Spine"]=(0,8*env*S(f*t-0.3),0); p["Hips"]=(0,0,6*env*S(f*t-0.4))
    p["Tail_1"]=(10,0,30*env*S(f*t-0.5))
    p["Ear_L"]=(0,-30*env*abs(S(f*t)),0); p["Ear_R"]=(0,30*env*abs(S(f*t)),0)
    p["Root"]=dict(loc=(0,0,-0.01*env))
    return p

def beg(t):
    """sitting up tall on the haunches, front paws tucked up"""
    p=sit_pose(1.0,t)
    p["Root"]=dict(loc=(0,-0.05,-0.03), rot=(-58,0,0), pivot_y=0.33)
    for s in ("L","R"):
        sg=1 if s=="L" else -1
        p["Front"+s+"_Upper"]=(-25,0,0); p["Front"+s+"_Lower"]=(60+6*S(2*t),0,0); p["Front"+s+"_Paw"]=(30,0,0)
        p["Back"+s+"_Upper"]=(-22,0,sg*16); p["Back"+s+"_Lower"]=(66,0,0); p["Back"+s+"_Paw"]=(14,0,0)
    p["Spine"]=(12,0,0); p["Chest"]=(10,0,0); p["Neck"]=(18,0,0); p["Head"]=(14+2*S(t),0,0)
    return p

def spin(t):
    """one-shot: happy 360 turn on the spot with a hop"""
    p=walk((t*3)%1.0)
    p["Root"]=dict(loc=(0,0,0.03*bump(t,0,1)), rot=(0,0,360*ease(t)), pivot_y=0.07)
    return p

def playbow(t):
    """front end down, bottom in the air"""
    p={}; w=S(2*t)
    p["Root"]=dict(loc=(0,0,-0.02), rot=(24,0,0), pivot_y=0.30)
    for s in ("L","R"):
        p["Front"+s+"_Upper"]=(-70,0,0); p["Front"+s+"_Lower"]=(-12,0,0); p["Front"+s+"_Paw"]=(0,0,0)
        p["Back"+s+"_Upper"]=(-24,0,0); p["Back"+s+"_Lower"]=(0,0,0)
    p["Neck"]=(-16,0,0); p["Head"]=(-14,4*w,0)
    p["Hips"]=(0,0,7*w); p["Tail_1"]=(30,0,30*S(4*t)); p["Tail_2"]=(10,0,15*S(4*t-0.1))
    return p

def sneeze(t):
    """one-shot: wind up ... a-choo!"""
    p={}; up=bump(t,0.0,0.55); snap=bump(t,0.5,0.8)
    p["Neck"]=(-12*up+22*snap,0,0); p["Head"]=(-14*up+26*snap,0,0)
    p["Chest"]=(-3*up+6*snap,0,0); p["Root"]=dict(loc=(0,0,-0.012*snap))
    p["Ear_L"]=(0,-25*snap,0); p["Ear_R"]=(0,25*snap,0)
    return p

def paw(t):
    """sitting, offering one front paw (shake hands / high five)"""
    p=sit_pose(1.0,t); k=0.85+0.15*S(2*t)
    p["FrontL_Upper"]=(26-88*k,0,0); p["FrontL_Lower"]=(4+20*k,0,0); p["FrontL_Paw"]=(-25*k,0,0)
    p["Head"]=(10,6,0)
    return p

# name, frames, loop, fn
CLIPS=[("Idle",60,True,idle),("Walk",24,True,walk),("Run",14,True,run),("Sit",60,True,sit),("Sleep",90,True,sleep),
       ("Eat",30,True,eat),("Happy",18,True,happy),("RollOver",48,False,rollover),("Dance",40,True,dance),
       ("Shake",36,False,shake),("Beg",60,True,beg),("Spin",30,False,spin),("PlayBow",60,True,playbow),
       ("Sneeze",30,False,sneeze),("Paw",60,True,paw)]
GAP=6
def layout():
    out=[]; f=0
    for n,fr,loop,fn in CLIPS:
        out.append((n,f,f+fr,loop,fn)); f+=fr+GAP
    return out
