using UnityEngine;
using Bella.Core;

namespace Bella.Systems
{
    public class OutfitDef
    {
        public string id, slot, shape, icon;
        public int price;
        public Color color, accent;

        public OutfitDef(string id, string slot, string shape, string icon, int price, Color color, Color accent)
        {
            this.id = id; this.slot = slot; this.shape = shape; this.icon = icon;
            this.price = price; this.color = color; this.accent = accent;
        }
    }

    public class TrickDef
    {
        public string id, clip, gesture;
        public bool loop;      // looped tricks are held for a moment, one-shots play once
        public float hold;

        public TrickDef(string id, string clip, string gesture, bool loop, float hold)
        {
            this.id = id; this.clip = clip; this.gesture = gesture; this.loop = loop; this.hold = hold;
        }
    }

    public class StickerDef
    {
        public string id, icon, title;
        public Color color;

        public StickerDef(string id, string icon, string title, Color color)
        {
            this.id = id; this.icon = icon; this.title = title; this.color = color;
        }
    }

    /// <summary>A food the Feeding shop sells. Spent treats buy one meal; free items cost nothing.</summary>
    public class FoodDef
    {
        public string id, icon;
        public int price;
        public float hunger, happiness;

        public FoodDef(string id, string icon, int price, float hunger, float happiness)
        {
            this.id = id; this.icon = icon; this.price = price; this.hunger = hunger; this.happiness = happiness;
        }
    }

    /// <summary>A wallpaper or floor for the Decor shop. `file` is the name under Resources/Surfaces.</summary>
    public class ThemeDef
    {
        public string id, kind, file;   // kind: "wall" or "floor"
        public int price;

        public ThemeDef(string id, string kind, string file, int price)
        {
            this.id = id; this.kind = kind; this.file = file; this.price = price;
        }
    }

    /// <summary>All of the game's content tables in one place — tune prices and add items here.</summary>
    public static class Catalog
    {
        public const int TricksToLearn = 3;

        public static readonly OutfitDef[] Outfits =
        {
            new OutfitDef("bow_pink",     "head", "bow",      "item_bow",      10, Art.Pink, Art.Rose),
            new OutfitDef("bandana_red",  "neck", "bandana",  "item_bandana",  10, Art.Rose, Color.white),
            new OutfitDef("hat_party",    "head", "partyhat", "item_partyhat", 15, Art.Sky, Art.Gold),
            new OutfitDef("bowtie_blue",  "neck", "bowtie",   "item_bow",      20, new Color(0.35f, 0.55f, 0.95f), Art.Gold),
            new OutfitDef("hat_top",      "head", "tophat",   "item_hat",      25, new Color(0.25f, 0.22f, 0.3f), Art.Rose),
            new OutfitDef("flower_sunny", "head", "flower",   "item_flower",   25, Color.white, Art.Gold),
            new OutfitDef("glasses_cool", "eyes", "glasses",  "item_glasses",  30, Art.Ink, new Color(0.2f, 0.25f, 0.35f)),
            new OutfitDef("glasses_rosy", "eyes", "glasses",  "item_glasses",  35, Art.Rose, Art.Pink),
            new OutfitDef("bandana_star", "neck", "bandana",  "item_bandana",  35, Art.Lilac, Art.Gold),
            new OutfitDef("cape_hero",    "back", "cape",     "item_cape",     50, Art.Rose, Art.Gold),
            new OutfitDef("cape_royal",   "back", "cape",     "item_cape",     60, new Color(0.5f, 0.35f, 0.8f), Art.Gold),
            new OutfitDef("crown_gold",   "head", "crown",    "item_crown",    80, Art.Gold, Art.Rose),
            new OutfitDef("bow_lilac",     "head", "bow",      "item_bow",      10, Art.Lilac, Art.Gold),
            new OutfitDef("bandana_navy",  "neck", "bandana",  "item_bandana",  15, new Color(0.15f, 0.2f, 0.4f), Color.white),
            new OutfitDef("glasses_gold",  "eyes", "glasses",  "item_glasses",  30, Art.Gold, Art.Ink),
            new OutfitDef("bowtie_gold",   "neck", "bowtie",   "item_bow",      20, Art.Gold, Art.Ink),
            new OutfitDef("flower_blue",   "head", "flower",   "item_flower",   25, Art.Sky, Art.Gold),
            new OutfitDef("cape_midnight", "back", "cape",     "item_cape",     55, new Color(0.12f, 0.14f, 0.25f), Art.Sky),
            new OutfitDef("crown_rose",    "head", "crown",    "item_crown",    85, Art.Rose, Art.Gold),
        };

        /// <summary>Wallpapers and floors. The original look is always free and never listed here.</summary>
        public static readonly ThemeDef[] Themes =
        {
            new ThemeDef("wall_paws",    "wall",  "wall_paws",    10),
            new ThemeDef("wall_clouds",  "wall",  "wall_clouds",  15),
            new ThemeDef("wall_hearts",  "wall",  "wall_hearts",  15),
            new ThemeDef("wall_sunny",   "wall",  "wall_sunny",   15),
            new ThemeDef("wall_rainbow", "wall",  "wall_rainbow", 20),
            new ThemeDef("wall_jungle",  "wall",  "wall_jungle",  20),
            new ThemeDef("wall_night",   "wall",  "wall_night",   25),
            new ThemeDef("wall_tiles",   "wall",  "wall_tiles",   25),
            new ThemeDef("floor_wood",    "floor", "floor_wood",    10),
            new ThemeDef("floor_checker", "floor", "floor_checker", 15),
            new ThemeDef("floor_carpet",  "floor", "floor_carpet",  20),
            new ThemeDef("floor_grass",   "floor", "floor_grass",   25),
            new ThemeDef("floor_sand",    "floor", "floor_sand",    25),
            new ThemeDef("floor_stage",   "floor", "floor_stage",   30),
        };

        public static ThemeDef FindTheme(string id)
        {
            for (int i = 0; i < Themes.Length; i++) if (Themes[i].id == id) return Themes[i];
            return null;
        }

        /// <summary>Foods for the Feeding shop (docs/GDD.md-style: buy with Treats, each feed is different).</summary>
        public static readonly FoodDef[] Foods =
        {
            new FoodDef("kibble",     "food_kibble", 0,  35f, 0f),
            new FoodDef("treat_meal", "food_treat",  8,  20f, 15f),
            new FoodDef("fancy_meal", "food_fancy",  20, 50f, 25f),
        };

        public static readonly TrickDef[] Tricks =
        {
            new TrickDef("sit",      "Sit",      "down",   true,  2.2f),
            new TrickDef("beg",      "Beg",      "up",     true,  2.4f),
            new TrickDef("paw",      "Paw",      "right",  true,  2.4f),
            new TrickDef("spin",     "Spin",     "left",   false, 0f),
            new TrickDef("rollover", "RollOver", "circle", false, 0f),
            new TrickDef("dance",    "Dance",    "zigzag", true,  3.2f),
        };

        public static readonly StickerDef[] Stickers =
        {
            new StickerDef("first_pet",    "heart",    "First cuddle",     Art.Pink),
            new StickerDef("first_meal",   "bone",     "First dinner",     Art.Orange),
            new StickerDef("first_bath",   "bath",     "First bath",       Art.Sky),
            new StickerDef("first_nap",    "zzz",      "First nap",        Art.Lilac),
            new StickerDef("arcade_1",     "game_blocks", "Game time",     Art.Orange),
            new StickerDef("arcade_10",    "game_puzzle", "Game lover",    Art.Lilac),
            new StickerDef("arcade_50",    "trophy",      "Game champion", Art.Gold),
            new StickerDef("visit_1",      "map",         "Little explorer", Art.Teal),
            new StickerDef("visit_all",    "rainbow",     "World traveller", Art.Gold),
            new StickerDef("first_catch",  "treat",    "Treat catcher",    Art.Orange),
            new StickerDef("catch_10",     "star",     "Catch champion",   Art.Gold),
            new StickerDef("first_dig",    "bone",     "Treasure hunter",  Art.Leaf),
            new StickerDef("dig_25",       "trophy",   "Master digger",    Art.Gold),
            new StickerDef("first_fetch",  "ball",     "First fetch",      Art.Leaf),
            new StickerDef("fetch_10",     "ball",     "Fetch fan",        Art.Orange),
            new StickerDef("fetch_50",     "trophy",   "Fetch champion",   Art.Gold),
            new StickerDef("first_trick",  "tricks",   "First trick",      Art.Gold),
            new StickerDef("all_tricks",   "trophy",   "Trick master",     Art.Gold),
            new StickerDef("first_outfit", "wardrobe", "Dressed up",       Art.Teal),
            new StickerDef("fashion_5",    "item_crown", "Fashion star",   Art.Pink),
            new StickerDef("first_checkup","clinic",   "Brave at the vet", Art.Sky),
            new StickerDef("first_gift",   "gift",     "First present",    Art.Pink),
            new StickerDef("streak_3",     "star",     "3 days together",  Art.Orange),
            new StickerDef("streak_7",     "rainbow",  "One whole week",   Art.Sky),
            new StickerDef("streak_14",    "sun",      "Two weeks",        Art.Gold),
            new StickerDef("streak_30",    "cake",     "Best friends",     Art.Pink),
            new StickerDef("grown_1",      "paw",      "Getting bigger",   Art.Teal),
            new StickerDef("grown_2",      "treat",    "Big puppy",        Art.Gold),
            new StickerDef("cuddles_50",   "sparkle",  "50 cuddles",       Art.Lilac),
            new StickerDef("butterfly_1",  "butterfly","Butterfly friend", Art.Sky),
            new StickerDef("butterfly_20", "rainbow",  "Garden of friends",Art.Leaf),
            new StickerDef("fest_lights",  "sparkle",  "Festival of Lights",  Art.Gold),
            new StickerDef("fest_colours", "rainbow",  "Festival of Colours", Art.Pink),
            new StickerDef("fest_winter",  "star",     "Winter holidays",     Art.Sky),
            new StickerDef("fest_birthday","cake",     "Bella's birthday",    Art.Lilac),
        };

        public static OutfitDef FindOutfit(string id)
        {
            for (int i = 0; i < Outfits.Length; i++) if (Outfits[i].id == id) return Outfits[i];
            return null;
        }

        public static FoodDef FindFood(string id)
        {
            for (int i = 0; i < Foods.Length; i++) if (Foods[i].id == id) return Foods[i];
            return null;
        }

        public static TrickDef FindTrickByGesture(string gesture)
        {
            for (int i = 0; i < Tricks.Length; i++) if (Tricks[i].gesture == gesture) return Tricks[i];
            return null;
        }

        public static StickerDef FindSticker(string id)
        {
            for (int i = 0; i < Stickers.Length; i++) if (Stickers[i].id == id) return Stickers[i];
            return null;
        }
    }
}
