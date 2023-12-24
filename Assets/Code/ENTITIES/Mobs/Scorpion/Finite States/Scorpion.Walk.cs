using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Scorpion : Enemy
{
    private class Walk : ScorpionBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Walk;

        private float _directionMultiplier = 1;

        private Coroutine _walkCoroutine;
        private Coroutine _attackCoroutine;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _walkModel = _scorpion.WalkModel;
        }

        public override void OnEnterBehaviour()
        {
            _attackModel.Tail.gameObject.SetActive(true);
            _walkCoroutine = _scorpion.StartCoroutine(WaitForWalkCdw());
            _attackCoroutine = _scorpion.StartCoroutine(WaitForAttackAgain());
            _attackModel.OnReadyToShootAgain.AddListener(OnReadyToShootAgain);

            GetRandomDirection();

            _scorpion.Animator.PlayAnimation(ScorpionAnimatorModel.AnimationName.Scorpion_Walk);
        }

        public override void OnExitBehaviour()
        {
            _attackModel.Tail.gameObject.SetActive(false);


            _scorpion.AttackCdwIndication.ForceEndCdw();
            _scorpion.StopCoroutine(_walkCoroutine);
            _scorpion.StopCoroutine(_attackCoroutine);
        }

        public override void Update()
        {
            if (CheckIfNeedToChangeDirection())
            {
                ChangeDirection();
                return;
            }
            _rigidbody2D.velocity = new Vector2(_scorpion.Status.MovementSpeed.Get() * _directionMultiplier, _rigidbody2D.velocity.y);
        }

        private void GetRandomDirection()
        {
            List<float> directions = new List<float>() { 1, -1 };
            _directionMultiplier = directions[Random.Range(0, 2)];

            _scorpion.RotationalTransform.localScale = new Vector3(_directionMultiplier, 1, 1);
        }

        private void ChangeDirection()
        {
            _directionMultiplier *= -1;
            _scorpion.RotationalTransform.localScale = new Vector3(_directionMultiplier, 1, 1);
        }

        private bool CheckIfNeedToChangeDirection()
        {
            Collider2D wall = _walkModel.WillHitTheWallCheck.DrawPhysics2D(_walkModel.LayerMask);
            if (wall != null) return true;

            Collider2D ground = _walkModel.WillHitTheGround.DrawPhysics2D(_walkModel.LayerMask);
            return ground == null;
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