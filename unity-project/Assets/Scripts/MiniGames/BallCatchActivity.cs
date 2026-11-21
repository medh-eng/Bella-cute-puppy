using Bella.Save;

namespace Bella.MiniGames
{
    /// <summary>Catch the Ball (park): the same slide-to-catch play as Treat Catch, but tennis balls and
    /// bouncy balls fly in from the sides. Counts towards the arcade stickers.</summary>
    public class BallCatchActivity : TreatCatchActivity
    {
        protected override string Place { get { return "park"; } }
        protected override string PlaceFloor { get { return "floor_grass"; } }
        protected override string HintText { get { return "Slide to catch the flying balls!"; } }
        protected override string NormalIcon { get { return "Icons/toy_tennis"; } }
        protected override string GoldIcon { get { return "Icons/ball"; } }
        protected override float Drift { get { return 1.1f; } }
        protected override void CountRound() { SaveSystem.Data.countArcade++; }
    }
}
