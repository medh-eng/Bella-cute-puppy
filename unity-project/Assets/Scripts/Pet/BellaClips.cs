namespace Bella.Pet
{
    /// <summary>
    /// Frame ranges of Bella's animations inside the single baked timeline of
    /// Resources/Bella/bella.fbx. GENERATED from tools/pipeline/clips.json — if the
    /// animations are re-exported, regenerate this table.
    /// </summary>
    public struct ClipDef
    {
        public string name;
        public int start;
        public int end;
        public bool loop;

        public ClipDef(string name, int start, int end, bool loop)
        {
            this.name = name; this.start = start; this.end = end; this.loop = loop;
        }

        public float Length => (end - start) / BellaClips.Fps;
        public float StartTime => start / BellaClips.Fps;
    }

    public static class BellaClips
    {
        public const float Fps = 30f;

        public static readonly ClipDef[] All =
        {
            new ClipDef("Idle", 0, 60, true),
            new ClipDef("Walk", 66, 90, true),
            new ClipDef("Run", 96, 110, true),
            new ClipDef("Sit", 116, 176, true),
            new ClipDef("Sleep", 182, 272, true),
            new ClipDef("Eat", 278, 308, true),
            new ClipDef("Happy", 314, 332, true),
            new ClipDef("RollOver", 338, 386, false),
            new ClipDef("Dance", 392, 432, true),
            new ClipDef("Shake", 438, 474, false),
            new ClipDef("Beg", 480, 540, true),
            new ClipDef("Spin", 546, 576, false),
            new ClipDef("PlayBow", 582, 642, true),
            new ClipDef("Sneeze", 648, 678, false),
            new ClipDef("Paw", 684, 744, true),
        };

        public static bool TryGet(string name, out ClipDef def)
        {
            for (int i = 0; i < All.Length; i++)
            {
                if (All[i].name == name) { def = All[i]; return true; }
            }
            def = All[0];
            return false;
        }
    }
}
