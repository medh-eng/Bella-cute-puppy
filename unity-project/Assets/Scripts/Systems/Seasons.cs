using System;
using System.Globalization;
using UnityEngine;
using Bella.Core;
using Bella.Save;

namespace Bella.Systems
{
    /// <summary>One festival: its art set (Resources/Seasons/&lt;id&gt;_1..9), music, sticker and greeting.</summary>
    public class FestivalDef
    {
        public readonly string id;          // also the art prefix: Seasons/lights_1.png ...
        public readonly string title;       // "Festival of Lights"
        public readonly string greeting;    // popup title
        public readonly string portrait;    // popup picture
        public readonly string music;       // replaces the morning / day / evening tunes (night keeps the lullaby)
        public readonly string sticker;
        public readonly Color color;

        public FestivalDef(string id, string title, string greeting, string portrait, string music, string sticker, Color color)
        {
            this.id = id; this.title = title; this.greeting = greeting; this.portrait = portrait;
            this.music = music; this.sticker = sticker; this.color = color;
        }
    }

    /// <summary>
    /// The festival calendar. Purely local: the phone's own date decides, nothing is fetched.
    /// While a festival is on, the living room is decorated (World/FestiveDecor), the daytime music
    /// changes, and the first visit of each festival brings a greeting, a present and a sticker
    /// (Progress.CheckFestival). Parents can switch festivals off in the parents' area.
    ///
    /// Diwali and Holi follow the Hindu lunar calendar, so their dates are a table (checked on
    /// 22 Nov 2026 against drikpanchang.com and timeanddate.com). After the last year in the table
    /// those two simply do not appear; extend the table in an update.
    /// </summary>
    public static class Seasons
    {
        public static readonly FestivalDef Lights = new FestivalDef("lights", "Festival of Lights",
            "Happy Festival of Lights!", "Portraits/face_love", "music_fest_lights", "fest_lights", Art.Gold);
        public static readonly FestivalDef Colours = new FestivalDef("colours", "Festival of Colours",
            "Happy Festival of Colours!", "Portraits/face_silly", "music_fest_colours", "fest_colours", Art.Pink);
        public static readonly FestivalDef Winter = new FestivalDef("winter", "Winter holidays",
            "Happy winter holidays!", "Portraits/face_chilly", "music_fest_winter", "fest_winter", Art.Sky);
        public static readonly FestivalDef Birthday = new FestivalDef("birthday", "Bella's birthday",
            "Happy birthday, Bella!", "Portraits/face_party", "music_fest_party", "fest_birthday", Art.Lilac);

        public static readonly FestivalDef[] All = { Lights, Colours, Winter, Birthday };

        // Main day of Diwali (Lakshmi Puja); decorations from 2 days before to 2 days after (the five days).
        private static readonly string[] DiwaliDays = { "2026-11-08", "2027-10-29", "2028-10-17", "2029-11-05", "2030-10-26" };
        // Rangwali Holi (the day of colours) in India; decorations from the day before to the day after.
        private static readonly string[] HoliDays = { "2026-03-04", "2027-03-22", "2028-03-11", "2029-03-01", "2030-03-20" };

        /// <summary>Editor-only preview (GameRoot: F key). -1 = follow the real calendar.</summary>
        public static int PreviewIndex = -1;

        /// <summary>The festival on today, or null. Respects the parents' switch.</summary>
        public static FestivalDef Today()
        {
            if (PreviewIndex >= 0 && PreviewIndex < All.Length) return All[PreviewIndex];
            if (!SaveSystem.Data.festivalsOn) return null;
            return On(DateTime.Now.Date, SaveSystem.Data.FirstPlayUtc.ToLocalTime().Date);
        }

        /// <summary>Which festival is on for a given local date (birthday wins when two overlap).</summary>
        public static FestivalDef On(DateTime date, DateTime firstPlay)
        {
            date = date.Date;
            if (IsBirthday(date, firstPlay)) return Birthday;
            if (Near(date, DiwaliDays, 2, 2)) return Lights;
            if (Near(date, HoliDays, 1, 1)) return Colours;
            if ((date.Month == 12 && date.Day >= 20) || (date.Month == 1 && date.Day == 1)) return Winter;
            return null;
        }

        /// <summary>
        /// A key that is the same for every day of one festival and different next year, so the
        /// greeting and present come once per festival ("winter-2026" covers 20 Dec 2026 – 1 Jan 2027).
        /// </summary>
        public static string OccurrenceKey(FestivalDef f, DateTime date)
        {
            int year = date.Year;
            if (f == Winter && date.Month == 1) year--;
            return f.id + "-" + year.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>Bella's birthday: the anniversary of the very first launch (and the day after), from her first year on.</summary>
        private static bool IsBirthday(DateTime date, DateTime firstPlay)
        {
            if (date < firstPlay.AddYears(1).AddDays(-1)) return false;
            for (int back = 0; back <= 1; back++)
            {
                DateTime d = date.AddDays(-back);
                if (d.Month == firstPlay.Month && d.Day == firstPlay.Day) return true;
                // A 29 February first launch celebrates on 28 February in ordinary years.
                if (firstPlay.Month == 2 && firstPlay.Day == 29 && !DateTime.IsLeapYear(d.Year) && d.Month == 2 && d.Day == 28) return true;
            }
            return false;
        }

        private static bool Near(DateTime date, string[] mainDays, int before, int after)
        {
            for (int i = 0; i < mainDays.Length; i++)
            {
                DateTime main = DateTime.ParseExact(mainDays[i], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (date >= main.AddDays(-before) && date <= main.AddDays(after)) return true;
            }
            return false;
        }

        public static FestivalDef Find(string id)
        {
            for (int i = 0; i < All.Length; i++) if (All[i].id == id) return All[i];
            return null;
        }
    }
}
