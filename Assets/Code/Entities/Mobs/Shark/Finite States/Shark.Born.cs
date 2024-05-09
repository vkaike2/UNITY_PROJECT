public partial class Shark : Enemy
{
    private class Born : SharkBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Born;

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