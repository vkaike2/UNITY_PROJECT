partial class StarFish : Enemy
{
    private class Born : StarFishBaseBehaviour
    {
        public override Behaviour Behaviour => StarFish.Behaviour.Born;

        public override void OnEnterBehaviour()
        {
            _starFish.Animator.PlayAnimation(StarFishAnimatorModel.AnimationName.Star_Fish_Born);
        }

        public override void OnExitBehaviour()
        {
        }

        public override void Update()
        {
        }
    }
}