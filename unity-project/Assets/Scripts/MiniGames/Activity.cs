using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.Pet;
using Bella.UI;

namespace Bella.MiniGames
{
    /// <summary>
    /// Base class for every mini-game. A mini-game takes over Bella and the
    /// camera, shows its own canvas with a big Back button, and hands everything
    /// back through Finish(). There are no fail states anywhere (docs/GDD.md §2).
    /// </summary>
    public abstract class Activity : MonoBehaviour
    {
        protected GameRoot G;
        protected BellaController Bella;
        protected Canvas Canvas;
        protected RectTransform UI;
        protected bool Ready;       // Bella has arrived and the game can accept input
        protected bool Ending;

        public bool Running { get; private set; }

        public void Begin(GameRoot root)
        {
            G = root;
            Bella = root.Bella;
            Running = true;
            Canvas = UIKit.CreateCanvas(GetType().Name + "Canvas", 20);
            Canvas.transform.SetParent(transform, false);
            UI = UIKit.Rect("Root", Canvas.transform);
            UIKit.Stretch(UI);

            Button back = UIKit.IconButton("Back", UI, "back", 140f, Color.white, Finish);
            UIKit.Place(back.GetComponent<RectTransform>(), new Vector2(0f, 1f), new Vector2(0.5f, 0.5f), new Vector2(110f, -120f), new Vector2(140f, 140f));

            AudioManager.Play("whoosh");
            OnBegin();
        }

        protected abstract void OnBegin();
        protected virtual void OnEnd() { }

        /// <summary>Leave the mini-game (Back button, Android back key, or natural end).</summary>
        public void Finish()
        {
            if (!Running) return;
            Running = false;
            StopAllCoroutines();
            try { OnEnd(); }
            catch (Exception e) { Diagnostics.Note(GetType().Name + " cleanup: " + e.Message); }
            if (Canvas != null) Destroy(Canvas.gameObject);
            G.ActivityFinished(this);
            Destroy(gameObject);
        }

        protected void FinishAfter(float seconds)
        {
            if (Ending) return;
            Ending = true;
            StartCoroutine(FinishRoutine(seconds));
        }

        private IEnumerator FinishRoutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Finish();
        }

        protected void After(float seconds, Action action)
        {
            StartCoroutine(AfterRoutine(seconds, action));
        }

        private IEnumerator AfterRoutine(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            if (Running && action != null) action();
        }

        /// <summary>Pointer position in this canvas's local space (origin = centre).</summary>
        protected Vector2 PointerLocal()
        {
            Vector2 local;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(UI, Pointer.Position, null, out local);
            return local;
        }

        protected bool PointerOnBella(out RaycastHit hit)
        {
            if (Pointer.Raycast(G.Cam, out hit))
                return hit.collider.GetComponentInParent<BellaController>() == Bella;
            return false;
        }
    }
}
