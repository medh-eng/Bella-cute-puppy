using System.Collections.Generic;
using UnityEngine;
using Bella.Core;
using Bella.Systems;

namespace Bella.Pet
{
    /// <summary>
    /// The Wardrobe's 3D side: builds each outfit piece from primitives and pins it
    /// to an anchor on Bella's skeleton so it follows her animation. One piece per
    /// slot (head / eyes / neck / back).
    /// </summary>
    public class Accessories : MonoBehaviour
    {
        private BellaModel model;
        private readonly Dictionary<string, GameObject> worn = new Dictionary<string, GameObject>(); // slot -> object
        private readonly Dictionary<string, string> wornId = new Dictionary<string, string>();       // slot -> item id

        public void Init(BellaModel m, List<string> equipped)
        {
            model = m;
            if (equipped == null) return;
            for (int i = 0; i < equipped.Count; i++)
            {
                OutfitDef def = Catalog.FindOutfit(equipped[i]);
                if (def != null) Equip(def);
            }
        }

        public bool IsWearing(string id)
        {
            foreach (KeyValuePair<string, string> kv in wornId)
                if (kv.Value == id) return true;
            return false;
        }

        public List<string> WornIds()
        {
            var list = new List<string>();
            foreach (KeyValuePair<string, string> kv in wornId) list.Add(kv.Value);
            return list;
        }

        public void Unequip(string slot)
        {
            GameObject go;
            if (worn.TryGetValue(slot, out go) && go != null) Destroy(go);
            worn.Remove(slot);
            wornId.Remove(slot);
        }

        public void Equip(OutfitDef def)
        {
            if (model == null || def == null) return;
            Unequip(def.slot);
            GameObject go = Build(def);
            if (go == null) return;
            worn[def.slot] = go;
            wornId[def.slot] = def.id;
        }

        private GameObject Build(OutfitDef def)
        {
            Transform anchor;
            switch (def.shape)
            {
                case "glasses": anchor = model.Eyes; break;
                case "bow": case "flower": anchor = model.EarSide; break;
                case "bandana": case "bowtie": anchor = model.NeckFront; break;
                case "cape": anchor = model.Back; break;
                default: anchor = model.HeadTop; break;
            }
            if (anchor == null) return null;

            var root = new GameObject("Outfit_" + def.id);
            Transform t = root.transform;
            t.SetParent(anchor, false);
            Color c = def.color;
            Color c2 = def.accent;

            switch (def.shape)
            {
                case "tophat":
                    Art.Prim(PrimitiveType.Cylinder, "Brim", t, new Vector3(0f, 0.01f, 0f), new Vector3(0.34f, 0.012f, 0.34f), c);
                    Art.Prim(PrimitiveType.Cylinder, "Crown", t, new Vector3(0f, 0.10f, 0f), new Vector3(0.21f, 0.09f, 0.21f), c);
                    Art.Prim(PrimitiveType.Cylinder, "Band", t, new Vector3(0f, 0.045f, 0f), new Vector3(0.22f, 0.022f, 0.22f), c2);
                    t.localRotation = Quaternion.Euler(-8f, 0f, 6f);
                    break;
                case "partyhat":
                    Art.Cone("Cone", t, new Vector3(0f, -0.01f, 0f), new Vector3(0.2f, 0.3f, 0.2f), c);
                    Art.Prim(PrimitiveType.Sphere, "Pom", t, new Vector3(0f, 0.3f, 0f), Vector3.one * 0.07f, c2);
                    Art.Prim(PrimitiveType.Cylinder, "Rim", t, new Vector3(0f, 0.0f, 0f), new Vector3(0.215f, 0.012f, 0.215f), c2);
                    t.localRotation = Quaternion.Euler(-6f, 0f, -8f);
                    break;
                case "crown":
                    Art.Prim(PrimitiveType.Cylinder, "Ring", t, new Vector3(0f, 0.03f, 0f), new Vector3(0.2f, 0.035f, 0.2f), c);
                    for (int i = 0; i < 5; i++)
                    {
                        float a = Mathf.PI * 2f * i / 5f;
                        var p = new Vector3(Mathf.Cos(a) * 0.09f, 0.065f, Mathf.Sin(a) * 0.09f);
                        Art.Cone("Spike", t, p, new Vector3(0.06f, 0.08f, 0.06f), c);
                        Art.Prim(PrimitiveType.Sphere, "Gem", t, p + new Vector3(0f, 0.085f, 0f), Vector3.one * 0.035f, c2);
                    }
                    t.localRotation = Quaternion.Euler(-8f, 0f, 0f);
                    break;
                case "bow":
                    Art.Prim(PrimitiveType.Sphere, "L", t, new Vector3(-0.07f, 0f, 0f), new Vector3(0.13f, 0.1f, 0.05f), c);
                    Art.Prim(PrimitiveType.Sphere, "R", t, new Vector3(0.07f, 0f, 0f), new Vector3(0.13f, 0.1f, 0.05f), c);
                    Art.Prim(PrimitiveType.Sphere, "Knot", t, Vector3.zero, Vector3.one * 0.055f, c2);
                    t.localRotation = Quaternion.Euler(0f, 0f, 20f);
                    break;
                case "flower":
                    Art.Prim(PrimitiveType.Sphere, "Centre", t, Vector3.zero, Vector3.one * 0.06f, c2);
                    for (int i = 0; i < 5; i++)
                    {
                        float a = Mathf.PI * 2f * i / 5f;
                        Art.Prim(PrimitiveType.Sphere, "Petal", t, new Vector3(Mathf.Cos(a) * 0.055f, Mathf.Sin(a) * 0.055f, -0.005f),
                            new Vector3(0.07f, 0.07f, 0.03f), c);
                    }
                    t.localRotation = Quaternion.Euler(-20f, 25f, 0f);
                    break;
                case "glasses":
                {
                    float ex = 0.105f;
                    for (int s = -1; s <= 1; s += 2)
                    {
                        Art.Prim(PrimitiveType.Cylinder, "Rim", t, new Vector3(s * ex, 0f, 0f), new Vector3(0.15f, 0.008f, 0.15f), c, new Vector3(90f, 0f, 0f));
                        GameObject lens = Art.Prim(PrimitiveType.Cylinder, "Lens", t, new Vector3(s * ex, 0f, 0.004f), new Vector3(0.125f, 0.008f, 0.125f), c2, new Vector3(90f, 0f, 0f));
                        lens.GetComponent<Renderer>().sharedMaterial = Art.Transparent(new Color(c2.r, c2.g, c2.b, 0.55f));
                    }
                    Art.Prim(PrimitiveType.Cube, "Bridge", t, new Vector3(0f, 0.01f, 0f), new Vector3(0.08f, 0.018f, 0.012f), c);
                    break;
                }
                case "bandana":
                    Art.Prim(PrimitiveType.Cube, "Cloth", t, new Vector3(0f, -0.05f, 0.012f), new Vector3(0.17f, 0.17f, 0.012f), c, new Vector3(-12f, 0f, 45f));
                    Art.Prim(PrimitiveType.Sphere, "Dot", t, new Vector3(0f, -0.06f, 0.028f), new Vector3(0.05f, 0.05f, 0.012f), c2);
                    break;
                case "bowtie":
                    Art.Prim(PrimitiveType.Sphere, "L", t, new Vector3(-0.06f, 0f, 0.02f), new Vector3(0.11f, 0.085f, 0.04f), c);
                    Art.Prim(PrimitiveType.Sphere, "R", t, new Vector3(0.06f, 0f, 0.02f), new Vector3(0.11f, 0.085f, 0.04f), c);
                    Art.Prim(PrimitiveType.Sphere, "Knot", t, new Vector3(0f, 0f, 0.025f), Vector3.one * 0.05f, c2);
                    break;
                case "cape":
                    Art.Prim(PrimitiveType.Cube, "Cloth", t, new Vector3(0f, -0.03f, -0.2f), new Vector3(0.34f, 0.012f, 0.46f), c, new Vector3(-14f, 0f, 0f));
                    Art.Prim(PrimitiveType.Sphere, "Clasp", t, new Vector3(0f, 0.01f, 0.03f), Vector3.one * 0.05f, c2);
                    break;
                default:
                    Destroy(root);
                    return null;
            }
            return root;
        }
    }
}
