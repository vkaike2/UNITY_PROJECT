using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Scorpion : Enemy
{
    private class Walk : ScorpionBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Walk;

        private Coroutine _walkCoroutine;
        private Coroutine _attackCoroutine;

        private EnemyPatrolBehaviour _patrolBehaviour;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _walkModel = _scorpion.WalkModel;

            _walkModel.OnChangeAnimationToWalk.AddListener(() =>
            {
                _scorpion.Animator.PlayAnimation(ScorpionAnimatorModel.AnimationName.Scorpion_Walk);
            });
            _patrolBehaviour = new EnemyPatrolBehaviour(EnemyPatrolBehaviour.MovementType.Walk, _walkModel);
            _patrolBehaviour.Start(enemy);
        }

        public override void OnEnterBehaviour()
        {
            _attackModel.Tail.gameObject.SetActive(true);
            _walkCoroutine = _scorpion.StartCoroutine(WaitForWalkCdw());
            _attackCoroutine = _scorpion.StartCoroutine(WaitForAttackAgain());
            _attackModel.OnReadyToShootAgain.AddListener(OnReadyToShootAgain);

            _patrolBehaviour.OnEnterBehaviour();
        }

        public override void OnExitBehaviour()
        {
            _attackModel.Tail.gameObject.SetActive(false);

            _scorpion.AttackCdwIndication.ForceEndCdw();
            _scorpion.StopCoroutine(_walkCoroutine);
            _scorpion.StopCoroutine(_attackCoroutine);

            _patrolBehaviour.OnExitBehaviour();
        }

        public override void Update()
        {
            _patrolBehaviour.Update();
        }

        private IEnumerator WaitForWalkCdw()
        {
            yield return new WaitForSeconds(_walkModel.CdwToMoveBackToIdle);
            _scorpion.ChangeBehaviour(Behaviour.Idle);
        }

        private IEnumerator WaitForAttackAgain()
        {
            _scorpion.AttackCdwIndication.StartCdw(_attackModel.CdwBetweenShoot);
            yield return new WaitForSeconds(_attackModel.CdwBetweenShoot);
            _attackModel.Tail.StartShoot();
        }

        private void OnReadyToShootAgain()
        {
            _attackCoroutine = _scorpion.StartCoroutine(WaitForAttackAgain());
        }

    }
}