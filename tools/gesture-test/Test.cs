using System; using System.Collections.Generic; using UnityEngine; using Bella.MiniGames;
static class T {
  static Random R=new Random(7);
  static float U(double a,double b)=>(float)(a+R.NextDouble()*(b-a));
  static float G(){ double u=1-R.NextDouble(),v=R.NextDouble(); return (float)(Math.Sqrt(-2*Math.Log(u))*Math.Sin(2*Math.PI*v)); }
  // resample a polyline as a finger would produce it: point every ~29px (screen.height*0.012) with jitter
  static List<Vector2> Finger(List<Vector2> path, float jitter){
    var o=new List<Vector2>(); float step=2400*0.012f; o.Add(path[0]); float acc=0;
    for(int i=1;i<path.Count;i++){ Vector2 a=path[i-1],b=path[i]; float len=(b-a).magnitude; float t=step-acc;
      while(t<=len){ var p=new Vector2(a.x+(b.x-a.x)*t/len+G()*jitter, a.y+(b.y-a.y)*t/len+G()*jitter); o.Add(p); t+=step; } acc=(acc+len)%step; }
    return o; }
  static List<Vector2> Dense(Func<float,Vector2> f,int n=400){ var l=new List<Vector2>(); for(int i=0;i<=n;i++) l.Add(f(i/(float)n)); return l; }
  static Vector2 C()=>new Vector2(U(300,780),U(700,1700));
  static List<Vector2> Line(string dir){ float len=U(320,1300); float tilt=U(-28,28)*(float)Math.PI/180; float bow=U(-0.12,0.12)*len; Vector2 c=C();
    float ang= dir=="right"?0: dir=="up"?90: dir=="left"?180:270; double a=ang*Math.PI/180+tilt; float dx=(float)Math.Cos(a),dy=(float)Math.Sin(a);
    return Finger(Dense(t=>new Vector2(c.x+dx*(t-0.5f)*len - dy*bow*(float)Math.Sin(t*Math.PI), c.y+dy*(t-0.5f)*len + dx*bow*(float)Math.Sin(t*Math.PI))), U(1,7)); }
  static List<Vector2> Wobbly(string dir){ float len=U(320,1300); float tilt=U(-22,22)*(float)Math.PI/180; float amp=U(0.02,0.06)*len; float per=U(1.0,2.0); float ph=U(0,6.28); Vector2 c=C();
    float ang= dir=="right"?0: dir=="up"?90: dir=="left"?180:270; double a=ang*Math.PI/180+tilt; float dx=(float)Math.Cos(a),dy=(float)Math.Sin(a);
    return Finger(Dense(t=>{ float w=amp*(float)Math.Sin(t*per*6.283+ph); return new Vector2(c.x+dx*(t-0.5f)*len - dy*w, c.y+dy*(t-0.5f)*len + dx*w); }), U(1,7)); }
  static List<Vector2> Circle(){ float r=U(150,480); float asp=U(0.6,1.0); bool flip=R.Next(2)==0; float start=U(0,6.283); float sweep=U(0.82,1.3)*6.283f*(R.Next(2)==0?1:-1); Vector2 c=C(); float drift=U(-0.15,0.15)*r;
    return Finger(Dense(t=>{ double a=start+sweep*t; float rr=r*(1+0.08f*(float)Math.Sin(3*a+start)); float x=(float)Math.Cos(a)*rr*(flip?asp:1), y=(float)Math.Sin(a)*rr*(flip?1:asp); return new Vector2(c.x+x+drift*t,c.y+y+drift*t); }), U(1,8)); }
  static List<Vector2> Zig(){ int segs=R.Next(3,7); bool horiz=R.Next(3)!=0; float span=U(450,950), amp=U(140,420); Vector2 c=C(); var pts=new List<Vector2>(); 
    for(int i=0;i<=segs;i++){ float along=(i/(float)segs-0.5f)*span + U(-25,25); float side=((i%2==0)?-1:1)*amp*0.5f*U(0.75,1.2); pts.Add(horiz? new Vector2(c.x+along,c.y+side): new Vector2(c.x+side,c.y+along)); }
    if(R.Next(2)==0) pts.Reverse();
    var dense=new List<Vector2>(); for(int i=1;i<pts.Count;i++) for(int k=0;k<40;k++){ float t=k/40f; dense.Add(new Vector2(pts[i-1].x+(pts[i].x-pts[i-1].x)*t, pts[i-1].y+(pts[i].y-pts[i-1].y)*t)); } dense.Add(pts[pts.Count-1]);
    return Finger(dense,U(1,7)); }
  static List<Vector2> Tap(){ Vector2 c=C(); var l=new List<Vector2>(); int n=R.Next(1,6); for(int i=0;i<n;i++) l.Add(new Vector2(c.x+G()*12,c.y+G()*12)); return l; }
  static void Run(string want, Func<List<Vector2>> gen, int n=4000){ var hist=new Dictionary<string,int>(); for(int i=0;i<n;i++){ string g=GestureRecognizer.Recognize(gen()); if(g=="")g="(none)"; hist[g]=hist.TryGetValue(g,out int v)?v+1:1; }
    int ok=hist.TryGetValue(want,out int o)?o:0; Console.Write($"{want,-8} {100.0*ok/n,5:0.0}%  "); foreach(var kv in hist) if(kv.Key!=want) Console.Write($"{kv.Key}:{100.0*kv.Value/n:0.0}% "); Console.WriteLine(); }
  static void Main(){ foreach(var d in new[]{"up","down","left","right"}){ string dd=d; Run(dd,()=>Line(dd)); } foreach(var d in new[]{"up","right"}){ string dd=d; Console.Write("wobbly "); Run(dd,()=>Wobbly(dd)); } Run("circle",Circle); Run("zigzag",Zig); Run("(none)",Tap); }
}
