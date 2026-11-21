"""Four icons for the arcade mini-games (arrow, blocks, puzzle, car), drawn in code. Output: Resources/Icons."""
from PIL import Image, ImageDraw
import os, math
S=4; N=192; W=N*S
OUT=os.path.join(os.path.dirname(__file__),"..","..","unity-project","Assets","Resources","Icons")
OL=(70,48,60,255); LW=6*S//4*1
def new(): return Image.new("RGBA",(W,W),(0,0,0,0))
def P(*v): return [x*S for x in v]
def save(im,name): im.resize((256,256),Image.LANCZOS).save(os.path.join(OUT,name+".png"))
def circ(d,cx,cy,r,fill): d.ellipse(P(cx-r,cy-r,cx+r,cy+r),fill=fill,outline=OL,width=LW)
def rr(d,x0,y0,x1,y1,r,fill): d.rounded_rectangle(P(x0,y0,x1,y1),radius=r*S,fill=fill,outline=OL,width=LW)

# target with arrow
im=new(); d=ImageDraw.Draw(im)
for r,c in [(78,(255,255,255,255)),(60,(240,90,100,255)),(42,(255,255,255,255)),(24,(240,90,100,255)),(9,(255,205,70,255))]: circ(d,96,100,r,c)
d.line(P(96,100,168,28),fill=(150,100,60,255),width=9*S//2)
d.polygon(P(96,100,108,96,100,108),fill=OL)
d.polygon(P(168,28,184,26,170,40),fill=(90,190,150,255),outline=OL)
d.polygon(P(160,36,176,34,164,46),fill=(90,190,150,255),outline=OL)
save(im,"game_arrow")

# blocks
im=new(); d=ImageDraw.Draw(im)
cells=[(0,1,(90,190,230,255)),(1,1,(90,190,230,255)),(2,1,(90,190,230,255)),(2,0,(90,190,230,255)),
       (0,2,(250,170,70,255)),(1,2,(250,170,70,255)),(1,3,(250,170,70,255)),(2,3,(250,170,70,255)),
       (3,3,(240,90,100,255)),(3,2,(240,90,100,255)),(3,1,(240,90,100,255)),(4,1,(110,200,120,255)),(4,2,(110,200,120,255))]
for cx,cy,c in cells: rr(d,18+cx*30,24+cy*30,18+cx*30+28,24+cy*30+28,6,c)
save(im,"game_blocks")

# puzzle piece
im=new(); d=ImageDraw.Draw(im)
col=(150,110,230,255)
rr(d,40,56,152,152,10,col)
circ(d,96,50,17,col); circ(d,160,104,17,col)
rr(d,42,58,150,150,8,col)
d.ellipse(P(26,92,54,120),fill=(0,0,0,0),outline=OL,width=LW)
d.ellipse(P(78,38,114,62),fill=col)
d.ellipse(P(146,90,176,118),fill=col)
d.ellipse(P(30,92,56,118),fill=(0,0,0,0))
save(im,"game_puzzle")

# car
im=new(); d=ImageDraw.Draw(im)
rr(d,20,92,172,142,16,(240,90,100,255))
d.polygon(P(52,92,76,56,128,56,150,92),fill=(240,90,100,255),outline=OL)
d.polygon(P(62,90,80,64,94,64,94,90),fill=(190,230,250,255),outline=OL)
d.polygon(P(100,90,100,64,124,64,140,90),fill=(190,230,250,255),outline=OL)
circ(d,60,144,20,(70,60,80,255)); circ(d,60,144,8,(220,220,230,255))
circ(d,134,144,20,(70,60,80,255)); circ(d,134,144,8,(220,220,230,255))
circ(d,166,112,8,(255,225,110,255))
save(im,"game_car")
print("ok")
