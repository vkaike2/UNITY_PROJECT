public partial class Shark : Enemy
{
    private class Die : SharkBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Die;

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