using System;

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
            _attackModel.PlayerOnRangeCheck.OnLayerCheckTriggerEnter.AddListener(OnPlayerInAttackRange);
            _patrolBehaviour.OnEnterBehaviour();
        }

        public override void OnExitBehaviour()
        {
            _attackModel.PlayerOnRangeCheck.OnLayerCheckTriggerEnter.RemoveListener(OnPlayerInAttackRange);
            _patrolBehaviour.OnExitBehaviour();
        }

        public override void Update()
        {
            _patrolBehaviour.Update();
        }

        private void OnPlayerInAttackRange(UnityEngine.GameObject collidingWith)
        {
            if(!_attackModel.CanAttack) return;
            
            _shark.ChangeBehaviour(Behaviour.Attack);
        }
    }
}