public partial class FlyingFish : Enemy
{
    private class Floor : FlyingFishBaseBehaviour
    {
        public override Behaviour Behaviour => FlyingFish.Behaviour.Floor;

        public override void OnEnterBehaviour()
        {
        }

        public override void OnExitBehaviour()
        {
        }

        public override void Update()
        {
        }
    }
}