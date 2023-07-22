using System;
using System.Collections;
using UnityEngine;

public partial class Bird : Enemy
{
    private class Patrol : BirdBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Patrol;

        private BirdPatrolModel _model;


        public override void Start(Enemy enemy)
        {
            base.Start(enemy);

            _model = _bird.PatrolModel;
            _model.InitialHorizontalPosition = _bird.transform.position.y;
        }

        public override void OnEnterBehaviour()
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _bird.BirdAnimator.PlayAnimation(BridAnimatorModel.AnimationName.Bird_Fly);

            _model.OnFlappingWings.AddListener(OnFlappingWings);
            _bird.StartCoroutine(WaitForAtkCdw());
        }

        public override void OnExitBehaviour()
        {
            _model.OnFlappingWings.RemoveListener(OnFlappingWings);
        }

        public override void Update()
        {
            HorizontalMovement();
            FlipIfCollideWithWall();
        }

        private void FlipIfCollideWithWall()
        {
            if (_model.CollidingWithWall.IsRaycastCollidingWithLayer)
            {
                _bird.RotationalTransform.localScale = new Vector3(-_bird.RotationalTransform.localScale.x, 1, 1);
            }
        }

        private void HorizontalMovement()
        {
            float velocity = _bird.Status.MovementSpeed.Get();
            if (_bird.RotationalTransform.localScale.x == -1)
            {
                velocity *= -1;
            }

            _rigidbody2D.velocity = new Vector2(velocity, _rigidbody2D.velocity.y);
        }

        private void OnFlappingWings()
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _model.HorizontalVelocity);
        }

        private IEnumerator WaitForAtkCdw()
        {
            yield return new WaitForSeconds(_model.AtkCdw);

            _bird.AtkModel.TargetPosition = _bird.GameManager.Player.transform.position;
            _bird.ChangeBehaviour(Behaviour.Atk);
        }

    }
}
