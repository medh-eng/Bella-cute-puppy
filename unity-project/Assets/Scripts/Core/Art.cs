using System.Collections.Generic;
using UnityEngine;

namespace Bella.Core
{
    /// <summary>
    /// Runtime art helpers: materials, primitives, icons and fonts. Everything the
    /// game shows is built in code from a handful of Resources files, so there is
    /// no Inspector wiring anywhere in the project.
    /// </summary>
    public static class Art
    {
        private static Material litTemplate;
        private static Material transparentTemplate;
        private static Material spriteTemplate;
        private static Font font;
        private static Sprite panelSprite;
        private static Mesh coneMesh;
        private static readonly Dictionary<string, Sprite> iconCache = new Dictionary<string, Sprite>();
        private static readonly Dictionary<string, Texture2D> texCache = new Dictionary<string, Texture2D>();

        // ---------------------------------------------------------------- materials

        private static Material Template(ref Material cache, string resourceName, string shaderName, string fallbackShader)
        {
            if (cache != null) return cache;
            cache = Resources.Load<Material>("Materials/" + resourceName);
            if (cache == null)
            {
                Shader s = Shader.Find(shaderName);
                if (s == null) s = Shader.Find(fallbackShader);
                if (s == null) s = Shader.Find("Sprites/Default");
                cache = new Material(s);
                Diagnostics.Note("Material template '" + resourceName + "' not found in Resources; using Shader.Find.");
            }
            return cache;
        }

        /// <summary>Matte, cartoon-flat lit material.</summary>
        public static Material Lit(Color color, Texture texture = null)
        {
            var m = new Material(Template(ref litTemplate, "BellaLit", "Legacy Shaders/Diffuse", "Standard"));
            m.color = color;
            if (texture != null) m.mainTexture = texture;
            return m;
        }

        public static Material Transparent(Color color)
        {
            var m = new Material(Template(ref transparentTemplate, "BellaTransparent", "Legacy Shaders/Transparent/Diffuse", "Sprites/Default"));
            m.color = color;
            return m;
        }

        /// <summary>Unlit, alpha-blended (particles, glows, blob shadow).</summary>
        public static Material SpriteMat(Color color, Texture texture = null)
        {
            var m = new Material(Template(ref spriteTemplate, "BellaSprite", "Sprites/Default", "Unlit/Transparent"));
            m.color = color;
            if (texture != null) m.mainTexture = texture;
            return m;
        }

        // ---------------------------------------------------------------- primitives

        public static GameObject Prim(PrimitiveType type, string name, Transform parent,
            Vector3 localPos, Vector3 localScale, Color color, Vector3? localEuler = null, bool keepCollider = false)
        {
            GameObject go = GameObject.CreatePrimitive(type);
            go.name = name;
            if (!keepCollider)
            {
                Collider c = go.GetComponent<Collider>();
                if (c != null) Object.Destroy(c);
            }
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPos;
            go.transform.localScale = localScale;
            if (localEuler.HasValue) go.transform.localRotation = Quaternion.Euler(localEuler.Value);
            Renderer r = go.GetComponent<Renderer>();
            if (r != null) r.sharedMaterial = Lit(color);
            return go;
        }

        public static GameObject Cone(string name, Transform parent, Vector3 localPos, Vector3 localScale, Color color)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPos;
            go.transform.localScale = localScale;
            go.AddComponent<MeshFilter>().sharedMesh = ConeMesh();
            go.AddComponent<MeshRenderer>().sharedMaterial = Lit(color);
            return go;
        }

        /// <summary>Unit cone: base radius 0.5 at y = 0, tip at y = 1.</summary>
        private static Mesh ConeMesh()
        {
            if (coneMesh != null) return coneMesh;
            const int seg = 20;
            var verts = new List<Vector3>();
            var tris = new List<int>();
            for (int i = 0; i < seg; i++)
            {
                float a0 = Mathf.PI * 2f * i / seg;
                float a1 = Mathf.PI * 2f * (i + 1) / seg;
                var p0 = new Vector3(Mathf.Cos(a0) * 0.5f, 0f, Mathf.Sin(a0) * 0.5f);
                var p1 = new Vector3(Mathf.Cos(a1) * 0.5f, 0f, Mathf.Sin(a1) * 0.5f);
                int b = verts.Count;
                verts.Add(p0); verts.Add(new Vector3(0f, 1f, 0f)); verts.Add(p1);   // side
                tris.Add(b); tris.Add(b + 1); tris.Add(b + 2);
                b = verts.Count;
                verts.Add(p0); verts.Add(p1); verts.Add(Vector3.zero);                // base
                tris.Add(b); tris.Add(b + 1); tris.Add(b + 2);
            }
            coneMesh = new Mesh();
            coneMesh.name = "BellaCone";
            coneMesh.SetVertices(verts);
            coneMesh.SetTriangles(tris, 0);
            coneMesh.RecalculateNormals();
            coneMesh.RecalculateBounds();
            return coneMesh;
        }

        /// <summary>A flat disc lying on the ground that always renders unlit (blob shadow, rug glow).</summary>
        public static GameObject GroundDisc(string name, Transform parent, Vector3 localPos, float diameter, Color color)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.name = name;
            Collider c = go.GetComponent<Collider>();
            if (c != null) Object.Destroy(c);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPos;
            go.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            go.transform.localScale = new Vector3(diameter, diameter, 1f);
            Renderer r = go.GetComponent<Renderer>();
            r.sharedMaterial = SpriteMat(color, Tex("Icons/circle"));
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            r.receiveShadows = false;
            return go;
        }

        // ---------------------------------------------------------------- textures / sprites / font

        public static Texture2D Tex(string resourcePath)
        {
            Texture2D t;
            if (texCache.TryGetValue(resourcePath, out t) && t != null) return t;
            t = Resources.Load<Texture2D>(resourcePath);
            if (t == null)
            {
                Diagnostics.Note("Missing texture: Resources/" + resourcePath);
                t = Texture2D.whiteTexture;
            }
            else
            {
                t.wrapMode = TextureWrapMode.Clamp;
            }
            texCache[resourcePath] = t;
            return t;
        }

        public static Sprite Icon(string name)
        {
            return SpriteAt("Icons/" + name);
        }

        public static Sprite SpriteAt(string resourcePath)
        {
            Sprite s;
            if (iconCache.TryGetValue(resourcePath, out s) && s != null) return s;
            Texture2D t = Tex(resourcePath);
            s = UnityEngine.Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f), 100f);
            s.name = resourcePath;
            iconCache[resourcePath] = s;
            return s;
        }

        /// <summary>Rounded 9-slice panel sprite.</summary>
        public static Sprite Panel()
        {
            if (panelSprite != null) return panelSprite;
            Texture2D t = Tex("Icons/panel");
            float b = Mathf.Floor(t.width * 0.34f);
            if (t.width < 8) b = 0f;
            panelSprite = UnityEngine.Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f),
                100f, 0, SpriteMeshType.FullRect, new Vector4(b, b, b, b));
            panelSprite.name = "BellaPanel";
            return panelSprite;
        }

        public static Font GetFont()
        {
            if (font != null) return font;
            try { font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); } catch (System.Exception) { font = null; }
            if (font == null)
            {
                try { font = Resources.GetBuiltinResource<Font>("Arial.ttf"); } catch (System.Exception) { font = null; }
            }
            if (font == null) font = UnityEngine.Font.CreateDynamicFontFromOSFont("Arial", 32);
            return font;
        }

        public static Transform FindDeep(Transform root, string name)
        {
            if (root == null) return null;
            if (root.name == name) return root;
            for (int i = 0; i < root.childCount; i++)
            {
                Transform t = FindDeep(root.GetChild(i), name);
                if (t != null) return t;
            }
            return null;
        }

        // Palette shared across UI and room.
        public static readonly Color Cream = new Color(1.00f, 0.96f, 0.87f);
        public static readonly Color Ink = new Color(0.29f, 0.19f, 0.14f);
        public static readonly Color Teal = new Color(0.49f, 0.84f, 0.77f);
        public static readonly Color Pink = new Color(1.00f, 0.59f, 0.69f);
        public static readonly Color Gold = new Color(1.00f, 0.84f, 0.25f);
        public static readonly Color Sky = new Color(0.66f, 0.85f, 0.98f);
        public static readonly Color Orange = new Color(1.00f, 0.67f, 0.35f);
        public static readonly Color Lilac = new Color(0.74f, 0.66f, 0.95f);
        public static readonly Color Leaf = new Color(0.47f, 0.82f, 0.47f);
        public static readonly Color Rose = new Color(0.94f, 0.33f, 0.38f);
    }
}
