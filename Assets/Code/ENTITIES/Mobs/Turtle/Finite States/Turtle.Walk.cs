using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Turtle : Enemy
{
    private class Walk : TurtleBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Walk;

        private float _directionMultiplier = 1;

        private TurtleWalkModel _walkModel;
        private Coroutine _gunCoroutine;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _walkModel = _turtle.WalkModel;
        }

        public override void OnEnterBehaviour()
        {
            _walkModel.TurtleGun.gameObject.SetActive(true);
            GetRandomDirection();
            _turtle.Animator.PlayAnimation(TurtleAnimatorModel.AnimationName.Turtle_Walk);
            _gunCoroutine = _turtle.StartCoroutine(ManageGun());

            _walkModel.OnRestartShoot.AddListener(RestartShoot);
        }

        public override void OnExitBehaviour()
        {
            _walkModel.OnRestartShoot.RemoveListener(RestartShoot);

            _walkModel.TurtleGun.gameObject.SetActive(false);
            _turtle.CdwIndication.ForceEndCdw();

            _turtle.StopCoroutine(_gunCoroutine);
        }

        public override void Update()
        {
            if (CheckIfNeedToChangeDirection())
            {
                ChangeDirection();
                return;
            }
            _rigidbody2D.velocity = new Vector2(_turtle.Status.MovementSpeed.Get() * _directionMultiplier, _rigidbody2D.velocity.y);
        }

        private void GetRandomDirection()
        {
            List<float> directions = new List<float>() { 1, -1 };
            _directionMultiplier = directions[Random.Range(0, 2)];

            _turtle.RotationalTransform.localScale = new Vector3(_directionMultiplier, 1, 1);
        }

        private void ChangeDirection()
        {
            _directionMultiplier *= -1;
            _turtle.RotationalTransform.localScale = new Vector3(_directionMultiplier, 1, 1);
        }

        private bool CheckIfNeedToChangeDirection()
        {
            Collider2D wall = _walkModel.WillHitTheWallCheck.DrawPhysics2D(_walkModel.LayerMask);
            if (wall != null) return true;

            Collider2D ground = _walkModel.WillHitTheGround.DrawPhysics2D(_walkModel.LayerMask);
            return ground == null;
        }


        private void RestartShoot()
        {
            _turtle.StartCoroutine(ManageGun());
        }

        private IEnumerator ManageGun()
        {
            _turtle.CdwIndication.StartCdw(_turtle.CdwBetweenShoots);

            yield return new WaitForSeconds(_turtle.CdwBetweenShoots);
            _walkModel.TurtleGun.Shoot(_turtle.ProjectileSpeed, _turtle.ProjectileDuration);
        }
    }
}