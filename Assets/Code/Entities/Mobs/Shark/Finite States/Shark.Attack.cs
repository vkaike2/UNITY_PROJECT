partial class Shark : Enemy
{
    private class Attack : SharkBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Attack;

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