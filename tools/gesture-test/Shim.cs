using System;
namespace UnityEngine {
  public struct Vector2 { public float x,y; public Vector2(float x,float y){this.x=x;this.y=y;}
    public float magnitude => (float)Math.Sqrt(x*x+y*y);
    public static Vector2 operator -(Vector2 a, Vector2 b)=>new Vector2(a.x-b.x,a.y-b.y);
    public static Vector2 operator +(Vector2 a, Vector2 b)=>new Vector2(a.x+b.x,a.y+b.y);
    public static float SignedAngle(Vector2 from, Vector2 to){ double d=from.x*to.x+from.y*to.y; double c=from.x*to.y-from.y*to.x; double m=Math.Sqrt((double)(from.x*from.x+from.y*from.y)*(to.x*to.x+to.y*to.y)); if(m<1e-15) return 0; double a=Math.Acos(Math.Max(-1,Math.Min(1,d/m)))*180/Math.PI; return (float)(a*Math.Sign(c)); } }
  public static class Mathf { public const float PI=3.14159265f; public static float Min(float a,float b)=>Math.Min(a,b); public static float Max(float a,float b)=>Math.Max(a,b); public static int Max(int a,int b)=>Math.Max(a,b); public static float Abs(float a)=>Math.Abs(a); public static float Sqrt(float a)=>(float)Math.Sqrt(a); }
  public static class Screen { public static int width=1080, height=2400; }
}
