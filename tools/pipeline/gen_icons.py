"""Original flat cartoon UI icons for Bella, drawn in code (no third-party art)."""
from PIL import Image, ImageDraw, ImageFilter
import math, os
S=4; N=192; W=N*S; OUT="icons"
OL=(74,48,36,255); LW=7*S
def new(): 
    im=Image.new("RGBA",(W,W),(0,0,0,0)); return im,ImageDraw.Draw(im)
def P(*v): return [x*S for x in v]
def fin(im,name): im.resize((256,256),Image.LANCZOS).save(f"{OUT}/{name}.png")
def circ(d,cx,cy,r,fill,ol=OL,w=LW): d.ellipse(P(cx-r,cy-r,cx+r,cy+r),fill=fill,outline=ol,width=w)
def rrect(d,x0,y0,x1,y1,r,fill,ol=OL,w=LW): d.rounded_rectangle(P(x0,y0,x1,y1),radius=r*S,fill=fill,outline=ol,width=w)
def poly(d,pts,fill,ol=OL,w=LW):
    pp=[(x*S,y*S) for x,y in pts]; d.polygon(pp,fill=fill)
    d.line(pp+[pp[0],pp[1]],fill=ol,width=w,joint="curve")
def line(d,pts,col=OL,w=LW): 
    pp=[(x*S,y*S) for x,y in pts]; d.line(pp,fill=col,width=w,joint="curve")
    for x,y in (pp[0],pp[-1]): d.ellipse((x-w/2,y-w/2,x+w/2,y+w/2),fill=col)
def heart_pts(cx,cy,s):
    pts=[]
    for i in range(60):
        t=2*math.pi*i/60; x=16*math.sin(t)**3; y=13*math.cos(t)-5*math.cos(2*t)-2*math.cos(3*t)-math.cos(4*t)
        pts.append((cx+x*s,cy-y*s))
    return pts
def star_pts(cx,cy,R,r=0.48,n=5,rot=-90):
    return [(cx+(R if i%2==0 else R*r)*math.cos(math.radians(rot+i*180/n)),cy+(R if i%2==0 else R*r)*math.sin(math.radians(rot+i*180/n))) for i in range(2*n)]
def shine(d,x,y,r=7): d.ellipse(P(x-r,y-r,x+r,y+r),fill=(255,255,255,210))

# bone
im,d=new()
tmp=Image.new("RGBA",(W,W),(0,0,0,0)); t=ImageDraw.Draw(tmp)
for (cx,cy) in ((40,82),(40,110),(152,82),(152,110)): t.ellipse(P(cx-22,cy-22,cx+22,cy+22),fill=OL)
t.rectangle(P(40,78,152,114),fill=OL)
for (cx,cy) in ((40,82),(40,110),(152,82),(152,110)): t.ellipse(P(cx-15,cy-15,cx+15,cy+15),fill=(255,244,222,255))
t.rectangle(P(40,85,152,107),fill=(255,244,222,255))
tmp=tmp.rotate(-25,resample=Image.BICUBIC); im.alpha_composite(tmp); fin(im,"bone")
# bubbles (wash)
im,d=new()
for cx,cy,r in ((78,112,50),(136,62,30),(142,134,20)):
    circ(d,cx,cy,r,(168,226,255,255)); shine(d,cx-r*0.35,cy-r*0.35,r*0.2)
fin(im,"bubbles")
# zzz moon
im,d=new()
circ(d,84,104,58,(255,224,110,255))
d.ellipse(P(84-58+38,104-58-22,84+58+38,104+58-22),fill=(0,0,0,0))
tmp=Image.new("RGBA",(W,W),(0,0,0,0)); t=ImageDraw.Draw(tmp)
im2,d2=new(); circ(d2,84,104,58,(255,224,110,255))
mask=Image.new("L",(W,W),255); md=ImageDraw.Draw(mask); md.ellipse(P(84-52+40,104-52-24,84+52+40,104+52-24),fill=0)
im=Image.composite(im2,Image.new("RGBA",(W,W),(0,0,0,0)),mask); d=ImageDraw.Draw(im)
d.arc(P(84-52+40,104-52-24,84+52+40,104+52-24),100,215,fill=OL,width=LW)
for (x,y,s) in ((118,44,22),(150,82,16)):
    line(d,[(x,y),(x+s,y),(x,y+s),(x+s,y+s)],(96,120,220,255),6*S)
fin(im,"zzz")
# heart
im,d=new(); poly(d,heart_pts(96,92,4.3),(255,105,140,255)); shine(d,66,66,9); fin(im,"heart")
# health cross
im,d=new(); circ(d,96,96,76,(255,255,255,255)); 
d.rounded_rectangle(P(80,48,112,144),radius=8*S,fill=(240,84,96,255)); d.rounded_rectangle(P(48,80,144,112),radius=8*S,fill=(240,84,96,255)); fin(im,"health")
# paw coin (treat)
im,d=new(); circ(d,96,96,80,(255,200,72,255)); circ(d,96,96,62,(255,222,120,255),ol=(214,150,40,255),w=4*S)
pc=(150,92,36,255)
d.ellipse(P(70,92,122,140),fill=pc)
for cx,cy in ((62,82),(84,62),(108,62),(130,82)): d.ellipse(P(cx-12,cy-14,cx+12,cy+14),fill=pc)
shine(d,52,52,8); fin(im,"treat")
# star
im,d=new(); poly(d,star_pts(96,100,82),(255,214,64,255)); shine(d,74,74,8); fin(im,"star")
# gift
im,d=new(); rrect(d,34,84,158,170,10,(255,120,150,255)); rrect(d,24,58,168,92,10,(255,150,175,255))
d.rectangle(P(84,58,108,170),fill=(255,226,110,255)); d.line(P(84,58,84,170),fill=OL,width=4*S); d.line(P(108,58,108,170),fill=OL,width=4*S)
d.ellipse(P(52,22,96,62),fill=(255,226,110,255),outline=OL,width=LW); d.ellipse(P(96,22,140,62),fill=(255,226,110,255),outline=OL,width=LW); fin(im,"gift")
# shirt (wardrobe)
im,d=new(); poly(d,[(66,30),(126,30),(170,62),(148,92),(132,80),(132,164),(60,164),(60,80),(44,92),(22,62)],(120,200,180,255))
d.arc(P(72,10,120,52),0,180,fill=OL,width=LW); fin(im,"wardrobe")
# magic wand (tricks)
im,d=new(); line(d,[(40,156),(112,84)],OL,20*S); line(d,[(40,156),(112,84)],(150,110,220,255),10*S)
poly(d,star_pts(126,66,46),(255,214,64,255)); 
for x,y in ((46,56),(160,128),(76,104)): poly(d,star_pts(x,y,13,0.4,4),(255,255,255,255),w=3*S)
fin(im,"tricks")
# clinic bag
im,d=new(); d.arc(P(66,26,126,86),180,360,fill=OL,width=10*S); rrect(d,22,56,170,160,16,(255,255,255,255))
d.rounded_rectangle(P(84,74,108,142),radius=5*S,fill=(240,84,96,255)); d.rounded_rectangle(P(62,96,130,120),radius=5*S,fill=(240,84,96,255)); fin(im,"clinic")
# bath tub
im,d=new()
for cx,cy,r in ((62,58,20),(96,44,24),(130,60,18)): circ(d,cx,cy,r,(200,238,255,255),w=5*S)
d.pieslice(P(26,10,166,160),0,180,fill=(130,200,245,255),outline=OL,width=LW); rrect(d,14,74,178,100,12,(170,222,255,255))
line(d,[(56,160),(48,176)]); line(d,[(136,160),(144,176)]); fin(im,"bath")
# book (stickers)
im,d=new(); rrect(d,28,30,164,166,12,(255,170,90,255)); rrect(d,44,30,164,166,8,(255,244,222,255)); 
poly(d,star_pts(106,96,36),(255,214,64,255),w=5*S); line(d,[(44,30),(44,166)]); fin(im,"book")
# lock
im,d=new(); d.arc(P(58,22,134,112),180,360,fill=OL,width=14*S); rrect(d,38,80,154,170,16,(190,190,205,255)); circ(d,96,118,12,OL,w=0); d.rectangle(P(91,118,101,146),fill=OL); fin(im,"lock")
# back arrow / play arrow / check / cross / home / gear / sound
im,d=new(); circ(d,96,96,80,(255,255,255,255)); line(d,[(112,56),(72,96),(112,136)],OL,14*S); fin(im,"back")
im,d=new(); circ(d,96,96,80,(120,210,120,255)); line(d,[(58,98),(86,126),(136,68)],(255,255,255,255),16*S); fin(im,"check")
im,d=new(); circ(d,96,96,80,(245,110,110,255)); line(d,[(66,66),(126,126)],(255,255,255,255),16*S); line(d,[(126,66),(66,126)],(255,255,255,255),16*S); fin(im,"close")
im,d=new(); poly(d,[(96,24),(172,92),(150,92),(150,164),(42,164),(42,92),(20,92)],(255,190,110,255)); rrect(d,80,112,112,164,4,(150,92,60,255),w=5*S); fin(im,"home")
im,d=new()
tmp=Image.new("RGBA",(W,W),(0,0,0,0)); t=ImageDraw.Draw(tmp)
for a in range(0,360,45):
    x=96+70*math.cos(math.radians(a)); y=96+70*math.sin(math.radians(a)); t.ellipse(P(x-20,y-20,x+20,y+20),fill=OL)
t.ellipse(P(30,30,162,162),fill=OL); im.alpha_composite(tmp)
for a in range(0,360,45):
    x=96+70*math.cos(math.radians(a)); y=96+70*math.sin(math.radians(a)); d.ellipse(P(x-13,y-13,x+13,y+13),fill=(190,190,205,255))
d.ellipse(P(37,37,155,155),fill=(190,190,205,255)); circ(d,96,96,26,(255,255,255,255)); fin(im,"gear")
for name,on in (("sound_on",True),("sound_off",False)):
    im,d=new(); poly(d,[(28,76),(62,76),(104,40),(104,152),(62,116),(28,116)],(255,255,255,255))
    if on:
        d.arc(P(96,62,140,130),-60,60,fill=OL,width=LW); d.arc(P(96,36,172,156),-60,60,fill=OL,width=LW)
    else:
        line(d,[(126,72),(170,120)],(240,84,96,255),10*S); line(d,[(170,72),(126,120)],(240,84,96,255),10*S)
    fin(im,name)
# ball
im,d=new(); circ(d,96,96,78,(255,110,110,255)); d.pieslice(P(18,18,174,174),200,340,fill=(255,236,150,255)); d.pieslice(P(18,18,174,174),20,160,fill=(110,190,250,255)); circ(d,96,96,78,None); circ(d,96,96,16,(255,255,255,255),w=5*S); shine(d,60,56,9); fin(im,"ball")
# thermometer
im,d=new(); tmp=Image.new("RGBA",(W,W),(0,0,0,0)); t=ImageDraw.Draw(tmp)
t.rounded_rectangle(P(80,14,112,140),radius=16*S,fill=(255,255,255,255),outline=OL,width=LW); t.ellipse(P(66,116,126,176),fill=(240,84,96,255),outline=OL,width=LW)
t.rectangle(P(90,70,102,130),fill=(240,84,96,255))
for y in (40,58,76): t.line(P(100,y,112,y),fill=OL,width=4*S)
im.alpha_composite(tmp.rotate(-35,resample=Image.BICUBIC)); fin(im,"thermometer")
# bandage
im,d=new(); tmp=Image.new("RGBA",(W,W),(0,0,0,0)); t=ImageDraw.Draw(tmp)
t.rounded_rectangle(P(14,66,178,126),radius=30*S,fill=(250,205,160,255),outline=OL,width=LW); t.rounded_rectangle(P(68,66,124,126),radius=4*S,fill=(255,236,210,255),outline=OL,width=5*S)
for x in (82,96,110):
    for y in (84,96,108): t.ellipse(P(x-3,y-3,x+3,y+3),fill=(214,160,110,255))
im.alpha_composite(tmp.rotate(35,resample=Image.BICUBIC)); fin(im,"bandage")
# medicine bottle
im,d=new(); rrect(d,66,18,126,54,8,(255,255,255,255)); rrect(d,42,48,150,174,18,(150,220,170,255)); rrect(d,58,84,134,140,6,(255,255,255,255),w=5*S)
d.rounded_rectangle(P(90,92,102,132),radius=3*S,fill=(240,84,96,255)); d.rounded_rectangle(P(76,106,116,118),radius=3*S,fill=(240,84,96,255)); fin(im,"medicine")
# stethoscope-ish "ears" tool -> cotton swab; keep simple: ice pack / spoon. Use carrot-vitamin? -> "brush"
im,d=new(); tmp=Image.new("RGBA",(W,W),(0,0,0,0)); t=ImageDraw.Draw(tmp)
t.rounded_rectangle(P(84,90,108,178),radius=10*S,fill=(255,170,90,255),outline=OL,width=LW); t.rounded_rectangle(P(52,20,140,100),radius=18*S,fill=(255,236,210,255),outline=OL,width=LW)
for x in (72,90,108,122): t.line(P(x,34,x,84),fill=(214,160,110,255),width=4*S)
im.alpha_composite(tmp.rotate(30,resample=Image.BICUBIC)); fin(im,"brush")
# sponge, shower, towel
im,d=new(); rrect(d,26,56,166,150,26,(255,220,90,255))
for x,y,r in ((60,86,10),(96,112,13),(130,84,8),(128,126,9),(70,124,7)): d.ellipse(P(x-r,y-r,x+r,y+r),fill=(232,180,50,255))
fin(im,"sponge")
im,d=new(); line(d,[(150,170),(150,60),(96,60)],OL,18*S); line(d,[(150,170),(150,60),(96,60)],(190,190,205,255),8*S)
d.pieslice(P(36,30,136,130),180,360,fill=(190,190,205,255),outline=OL,width=LW); d.line(P(36,80,136,80),fill=OL,width=LW)
for x,dx in ((56,-8),(86,0),(116,8)):
    for k in (0,1): line(d,[(x+dx*k*0.6,100+k*34),(x+dx*(k*0.6+0.4),118+k*34)],(90,170,240,255),7*S)
fin(im,"shower")
im,d=new(); rrect(d,40,24,152,170,14,(255,150,175,255)); d.rectangle(P(47,112,145,134),fill=(255,255,255,255)); d.line(P(40,112,152,112),fill=OL,width=4*S); d.line(P(40,134,152,134),fill=OL,width=4*S)
for x in range(52,150,16): line(d,[(x,170),(x,182)],OL,5*S)
fin(im,"towel")
# sun, moon-plain, camera-free; hat, glasses, bow, bandana, crown, cape icons (white, tinted at runtime)
im,d=new()
for a in range(0,360,45): line(d,[(96+56*math.cos(math.radians(a)),96+56*math.sin(math.radians(a))),(96+82*math.cos(math.radians(a)),96+82*math.sin(math.radians(a)))],(255,190,60,255),10*S)
circ(d,96,96,46,(255,220,90,255)); fin(im,"sun")
Wt=(255,255,255,255)
im,d=new(); d.pieslice(P(46,34,146,170),180,360,fill=Wt,outline=OL,width=LW); rrect(d,18,96,174,124,14,Wt); fin(im,"item_hat")
im,d=new(); circ(d,58,100,36,(210,235,255,255),w=10*S); circ(d,134,100,36,(210,235,255,255),w=10*S); line(d,[(88,92),(104,92)],OL,10*S); fin(im,"item_glasses")
im,d=new(); poly(d,[(96,96),(30,50),(30,142)],Wt); poly(d,[(96,96),(162,50),(162,142)],Wt); circ(d,96,96,20,Wt); fin(im,"item_bow")
im,d=new(); poly(d,[(22,54),(170,54),(96,160)],Wt); rrect(d,16,40,176,66,10,Wt); fin(im,"item_bandana")
im,d=new(); poly(d,[(26,150),(20,58),(62,98),(96,40),(130,98),(172,58),(166,150)],Wt); 
for x,y in ((20,58),(96,40),(172,58)): circ(d,x,y,10,(255,120,150,255),w=4*S)
fin(im,"item_crown")
im,d=new(); poly(d,[(60,30),(132,30),(170,168),(96,150),(22,168)],Wt); rrect(d,56,22,136,46,10,Wt); fin(im,"item_cape")
im,d=new(); poly(d,[(96,20),(150,120),(42,120)],Wt); circ(d,96,24,14,Wt,w=5*S); rrect(d,34,112,158,138,10,Wt); fin(im,"item_partyhat")
im,d=new(); 
for i,(cx,cy) in enumerate(((60,70),(132,70),(96,70))): poly(d,heart_pts(cx,cy+26,1.9) if False else star_pts(cx,cy+30,34 if i==2 else 26,0.5,5),Wt,w=5*S)
fin(im,"item_flower")
# thought bubble + rounded panel + soft circle (UI building blocks)
im,d=new(); d.ellipse(P(14,10,178,138),fill=Wt,outline=OL,width=LW); circ(d,58,150,14,Wt,w=5*S); circ(d,36,174,8,Wt,w=4*S); fin(im,"bubble")
im,d=new(); d.ellipse(P(4,4,188,188),fill=Wt); fin(im,"circle")
im=Image.new("RGBA",(W//2,W//2),(0,0,0,0)); d=ImageDraw.Draw(im); d.rounded_rectangle((0,0,W//2-1,W//2-1),radius=28*S,fill=Wt); im.resize((128,128),Image.LANCZOS).save(f"{OUT}/panel.png")
# sparkle, drop, germ (clinic), question
im,d=new(); poly(d,star_pts(96,96,84,0.28,4),(255,255,255,255),ol=(255,214,64,255),w=5*S); fin(im,"sparkle")
im,d=new(); poly(d,[(96,18)]+[(96+56*math.cos(math.radians(a)),116+56*math.sin(math.radians(a))) for a in range(-30,211,10)],(110,190,250,255)); shine(d,76,110,9); fin(im,"drop")
im,d=new(); circ(d,96,96,80,(255,255,255,255)); d.arc(P(66,46,126,106),190,90,fill=OL,width=14*S); line(d,[(96,104),(96,118)],OL,14*S); circ(d,96,146,9,OL,w=0); fin(im,"question")
# sticker-only art: pawprint, sunrise cake, trophy, rainbow
im,d=new(); circ(d,96,96,84,(255,236,210,255)); pc=(214,140,80,255); d.ellipse(P(64,92,128,150),fill=pc)
for cx,cy in ((52,80),(80,54),(112,54),(140,80)): d.ellipse(P(cx-14,cy-17,cx+14,cy+17),fill=pc)
fin(im,"paw")
im,d=new(); rrect(d,58,120,134,146,6,(255,214,64,255)); d.rectangle(P(86,96,106,122),fill=(255,214,64,255)); d.line(P(86,96,86,122),fill=OL,width=5*S); d.line(P(106,96,106,122),fill=OL,width=5*S)
d.pieslice(P(46,-20,146,104),0,180,fill=(255,214,64,255),outline=OL,width=LW); d.line(P(46,42,146,42),fill=OL,width=LW)
d.arc(P(18,40,62,84),90,270,fill=OL,width=LW); d.arc(P(130,40,174,84),270,90,fill=OL,width=LW); fin(im,"trophy")
im,d=new()
for i,c in enumerate(((240,84,96),(255,170,90),(255,220,90),(120,210,120),(110,190,250),(150,110,220))):
    r=84-i*11; d.pieslice(P(96-r,120-r,96+r,120+r),180,360,fill=c+(255,))
d.pieslice(P(96-18,120-18,96+18,120+18),180,360,fill=(0,0,0,0))
for cx in (26,166):
    for dx,dy,r in ((0,0,20),(-12,6,14),(12,6,14)): d.ellipse(P(cx+dx-r,122+dy-r,cx+dx+r,122+dy+r),fill=Wt)
fin(im,"rainbow")
im,d=new(); rrect(d,30,84,162,160,12,(255,236,210,255)); rrect(d,30,84,162,110,10,(255,150,175,255))
line(d,[(96,84),(96,52)],OL,12*S); line(d,[(96,84),(96,52)],(110,190,250,255),6*S); poly(d,[(96,24),(108,46),(96,56),(84,46)],(255,190,60,255),w=4*S); fin(im,"cake")
# gesture glyphs for Trick Training
GC=(110,120,235,255)
def arrow(d,a,b):
    line(d,[a,b],OL,26*S); 
    ang=math.atan2(b[1]-a[1],b[0]-a[0])
    h=[(b[0]-34*math.cos(ang-0.6),b[1]-34*math.sin(ang-0.6)),b,(b[0]-34*math.cos(ang+0.6),b[1]-34*math.sin(ang+0.6))]
    line(d,h,OL,26*S); line(d,[a,b],GC,14*S); line(d,h,GC,14*S)
for nm,(a,b) in {"down":((96,30),(96,160)),"up":((96,162),(96,32)),"left":((162,96),(32,96)),"right":((30,96),(160,96))}.items():
    im,d=new(); arrow(d,a,b); fin(im,"gesture_"+nm)
im,d=new(); d.arc(P(30,30,162,162),-60,250,fill=OL,width=26*S); d.arc(P(30,30,162,162),-60,250,fill=GC,width=14*S)
e=(96+66*math.cos(math.radians(250)),96+66*math.sin(math.radians(250))); h=[(e[0]-8,e[1]+38),e,(e[0]+36,e[1]+8)]
line(d,h,OL,26*S); line(d,h,GC,14*S); fin(im,"gesture_circle")
im,d=new(); z=[(24,130),(60,60),(96,130),(132,60),(168,130)]; line(d,z,OL,26*S); line(d,z,GC,14*S); fin(im,"gesture_zigzag")
# moon (clean crescent for the night window — no Z's), cloud, butterfly (ambience + sticker)
im2,d2=new(); circ(d2,88,100,62,(255,230,130,255))
mask=Image.new("L",(W,W),255); md=ImageDraw.Draw(mask); md.ellipse(P(88-56+42,100-56-24,88+56+42,100+56-24),fill=0)
im=Image.composite(im2,Image.new("RGBA",(W,W),(0,0,0,0)),mask); d=ImageDraw.Draw(im)
d.arc(P(88-56+42,100-56-24,88+56+42,100+56-24),105,210,fill=OL,width=LW)
poly(d,star_pts(142,58,15,0.45,4),(255,244,222,255),w=4*S); poly(d,star_pts(158,110,10,0.45,4),(255,244,222,255),w=3*S)
fin(im,"moon")
im,d=new()
tmp=Image.new("RGBA",(W,W),(0,0,0,0)); td=ImageDraw.Draw(tmp)
for cx,cy,r in ((62,106,34),(98,86,44),(136,108,32)): td.ellipse(P(cx-r,cy-r,cx+r,cy+r),fill=OL)
td.rounded_rectangle(P(30,104,168,142),radius=18*S,fill=OL)
for cx,cy,r in ((62,106,27),(98,86,37),(136,108,25)): td.ellipse(P(cx-r,cy-r,cx+r,cy+r),fill=(255,255,255,255))
td.rounded_rectangle(P(37,110,161,135),radius=13*S,fill=(255,255,255,255))
im.alpha_composite(tmp); shine(d,86,74,8); fin(im,"cloud")
im,d=new()
for sx in (-1,1):
    cx=96+sx*40
    d.ellipse(P(cx-32,36,cx+32,104),fill=(255,150,195,255),outline=OL,width=LW)      # top wing
    d.ellipse(P(96+sx*30-26,94,96+sx*30+26,152),fill=(190,160,245,255),outline=OL,width=LW)  # bottom wing
    circ(d,cx,70,11,(255,244,222,255),w=4*S)
d.rounded_rectangle(P(88,52,104,140),radius=8*S,fill=(120,86,60,255),outline=OL,width=5*S)
circ(d,96,46,14,(120,86,60,255),w=5*S)
line(d,[(90,38),(78,22)],OL,5*S); line(d,[(102,38),(114,22)],OL,5*S)
circ(d,78,22,5,(255,214,64,255),w=3*S); circ(d,114,22,5,(255,214,64,255),w=3*S)
fin(im,"butterfly")
print(len(os.listdir(OUT)),"icons")
# contact sheet
fs=sorted(os.listdir(OUT)); cols=10; rows=(len(fs)+cols-1)//cols
sh=Image.new("RGB",(cols*110,rows*110),(150,200,170))
for i,f in enumerate(fs): sh.paste(Image.open(f"{OUT}/{f}").resize((100,100)),((i%cols)*110+5,(i//cols)*110+5),Image.open(f"{OUT}/{f}").resize((100,100)))
sh.save("icons_sheet.png")
