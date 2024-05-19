using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public partial class PistolCrab : Enemy
{
    private class Walk : PistolCrabBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Walk;

        private EnemyPatrolBehaviour _patrolBehaviour;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);

            _walkModel.OnChangeAnimationToWalk.AddListener(() =>
            {
                _pistolCrab.Animator.PlayAnimation(PistolCrabAnimatorModel.AnimationName.PistolCrab_Walk);
            });

            _patrolBehaviour = new EnemyPatrolBehaviour(
                EnemyPatrolBehaviour.MovementType.Walk,
                _walkModel);

            _patrolBehaviour.Start(enemy);
        }

        public override void OnEnterBehaviour()
        {
            _patrolBehaviour.OnEnterBehaviour();
            _pistolCrab.StartCoroutine(WaitToGoBackToIdle());
        }

        public override void OnExitBehaviour()
        {
            _patrolBehaviour.OnExitBehaviour();
        }

        public override void Update()
        {
            _patrolBehaviour.Update();
        }

        private IEnumerator WaitToGoBackToIdle()
        {
            yield return new WaitForSeconds(_pistolCrab.WalkModel.CdwToGoBackToIdle);

            if (CanAttackPlayer())
            {
                _pistolCrab.ChangeBehaviour(Behaviour.Attack);
            }
            else
            {
                _pistolCrab.ChangeBehaviour(Behaviour.Idle);
            }
        }
    }
}