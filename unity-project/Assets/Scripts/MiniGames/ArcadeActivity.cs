using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.Pet;
using Bella.Save;
using Bella.Systems;
using Bella.UI;

namespace Bella.MiniGames
{
    /// <summary>
    /// Shared shell for the arcade mini-games: score and sub-score pills top right, a hint line, a Bella
    /// who watches and cheers behind the play field, and one payout routine that draws on the daily play pool.
    /// There is no losing: a round ends on time or when the moves run out, and always pays at least a little.
    /// </summary>
    public abstract class ArcadeActivity : Activity
    {
        protected Text HintLabel, ScoreLabel, SubLabel;
        protected bool Over;

        protected void BuildHud(string hint, string scoreIcon = "treat", bool showSub = true)
        {
            Image pill = UIKit.Panel("Score", UI, new Color(1f, 1f, 1f, 0.92f), false);
            UIKit.Place(pill.rectTransform, new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(-40f, -120f), new Vector2(270f, 104f));
            Image coin = UIKit.Icon("Icon", pill.transform, scoreIcon, 88f);
            UIKit.Place(coin.rectTransform, new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(56f, 0f), new Vector2(88f, 88f));
            ScoreLabel = UIKit.Label("Count", pill.transform, "0", 54, Art.Ink, TextAnchor.MiddleLeft);
            UIKit.Stretch(ScoreLabel.rectTransform, 112f, 0f, 14f, 0f);

            if (showSub)
            {
                Image sp = UIKit.Panel("Sub", UI, new Color(1f, 1f, 1f, 0.92f), false);
                UIKit.Place(sp.rectTransform, new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(-40f, -240f), new Vector2(270f, 84f));
                SubLabel = UIKit.Label("Sub", sp.transform, "", 46, Art.Ink, TextAnchor.MiddleCenter);
                UIKit.Stretch(SubLabel.rectTransform, 0f, 0f, 0f, 0f);
            }

            HintLabel = UIKit.Label("Hint", UI, hint, 50, Art.Ink, TextAnchor.MiddleCenter);
            UIKit.Place(HintLabel.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, -290f), new Vector2(960f, 100f));
            HintLabel.resizeTextForBestFit = true; HintLabel.resizeTextMinSize = 28; HintLabel.resizeTextMaxSize = 50;
        }

        protected void SetHint(string t) { if (HintLabel != null) HintLabel.text = t; }
        protected void SetScore(string t) { if (ScoreLabel != null) ScoreLabel.text = t; }
        protected void SetSub(string t) { if (SubLabel != null) SubLabel.text = t; }

        /// <summary>Camera on the park-sized close-up, Bella walks to the spot and holds a pose; then Ready = true.</summary>
        protected void Pose(string clip, System.Action ready = null)
        {
            G.CameraRig.Focus(new Vector3(0f, 0f, 0.25f), 2.9f, 0.45f);
            Bella.BeginActivity(new Vector3(0f, 0f, 0f), 180f, clip, () =>
            {
                Ready = true;
                Bella.Face.Set(Expression.Excited);
                if (ready != null) ready();
            });
        }

        protected void Cheer(int hearts = 4)
        {
            Bella.Puppy.PulseJoy();
            Fx.Hearts(Bella.HeadPoint + Vector3.up * 0.3f, hearts);
        }

        /// <summary>Pays from today's play pool, updates counters, celebrates and leaves after a moment.</summary>
        protected void EndRound(int wantTreats, string summary)
        {
            if (Over) return;
            Over = true;
            SaveData d = SaveSystem.Data;
            d.countArcade++;
            Bella.Needs.AddHappiness(12f);
            Bella.Needs.Tire(4f);
            wantTreats = Mathf.Clamp(wantTreats, 1, 12);
            int paid = Progress.PlayTreats(wantTreats);
            if (paid > 0) Bella.Needs.AddTreats(paid);
            SaveSystem.Flush();
            AudioManager.Voice("bark_happy");
            AudioManager.Play("success", 0.8f);
            Fx.Hearts(Bella.HeadPoint + Vector3.up * 0.3f, 12);
            Fx.Stars(Bella.HeadPoint + Vector3.up * 0.35f, 14);
            Fx.Confetti(Bella.HeadPoint + Vector3.up * 0.5f, 18);
            Bella.Anim.Play("Happy", 0.2f);
            SetHint(summary + (paid > 0 ? "   +" + paid + " treats" : "   (Enough treats for today!)"));
            FinishAfter(3.0f);
        }
    }
}
