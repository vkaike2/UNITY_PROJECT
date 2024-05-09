public partial class Shark : Enemy
{
    private class Walk : SharkBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Walk;

        private EnemyPatrolBehaviour _patrolBehaviour;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);

            _walkModel.OnChangeAnimationToWalk.AddListener(() =>
            {
                _shark.Animator.PlayAnimation(SharkAnimatorModel.AnimationName.Shark_Walk);
            });

            _patrolBehaviour = new EnemyPatrolBehaviour(
                EnemyPatrolBehaviour.MovementType.Walk,
                _walkModel);

            _patrolBehaviour.Start(enemy);
        }

        public override void OnEnterBehaviour()
        {
            _patrolBehaviour.OnEnterBehaviour();
        }

        public override void OnExitBehaviour()
        {
            _patrolBehaviour.OnExitBehaviour();
        }

        public override void Update()
        {
            _patrolBehaviour.Update();
        }
    }
}