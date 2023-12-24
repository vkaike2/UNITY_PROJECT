using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Turtle : Enemy
{
    private class Walk : TurtleBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Walk;

        private TurtleWalkModel _walkModel;
        private TurtleAttackModel _attackModel;
        private Coroutine _gunCoroutine;

        private EnemyPatrolBehaviour _patrolBehaviour;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _walkModel = _turtle.WalkModel;
            _attackModel = _turtle.AttackModel;

            _walkModel.OnChangeAnimationToWalk.AddListener(() =>
            {
                _turtle.Animator.PlayAnimation(TurtleAnimatorModel.AnimationName.Turtle_Walk);
            });

            _patrolBehaviour = new EnemyPatrolBehaviour(EnemyPatrolBehaviour.MovementType.Walk, _walkModel);
            _patrolBehaviour.Start(enemy);

        }

        public override void OnEnterBehaviour()
        {
            _attackModel.TurtleGun.gameObject.SetActive(true);
            _gunCoroutine = _turtle.StartCoroutine(ManageGun());
            _walkModel.OnRestartShoot.AddListener(RestartShoot);

            _patrolBehaviour.OnEnterBehaviour();
        }

        public override void OnExitBehaviour()
        {
            _walkModel.OnRestartShoot.RemoveListener(RestartShoot);
            _attackModel.TurtleGun.gameObject.SetActive(false);
            _turtle.CdwIndication.ForceEndCdw();
            _turtle.StopCoroutine(_gunCoroutine);

            _patrolBehaviour.OnExitBehaviour();
        }

        public override void Update()
        {
            _patrolBehaviour.Update();
        }

        private void RestartShoot()
        {
            _turtle.StartCoroutine(ManageGun());
        }

        private IEnumerator ManageGun()
        {
            _turtle.CdwIndication.StartCdw(_attackModel.CdwBetweenShoots);

            yield return new WaitForSeconds(_attackModel.CdwBetweenShoots);
            _attackModel.TurtleGun.Shoot(_attackModel.ProjectileSpeed, _attackModel.ProjectileDuration);
        }
    }
}