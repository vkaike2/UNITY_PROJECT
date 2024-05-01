using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class Armadillo : MonoBehaviour
{
    private class IntoBall : ArmadilloBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.IntoBall;

        private bool _canEnd = false;
        private float _initialGravity;
        private Vector2 _currentSpeed;
        private BasicCollider _basicBallCollider;

        public override void Start(Armadillo armadillo)
        {
            base.Start(armadillo);
            _initialGravity = _rigidBody2D.gravityScale;
            _basicBallCollider = _armadillo.Colliders.IntoBallCollider.GetComponent<BasicCollider>();
        }

        public override void OnEnterBehaviour()
        {
            _canEnd = false;
            _rigidBody2D.gravityScale = 0;
            _armadillo.StartCoroutine(TurnIntoBall());
        }

        public override void OnExitBehaviour()
        {
            _rigidBody2D.gravityScale = _initialGravity;
            _basicBallCollider.OnCollision.RemoveListener(CalculateNextDirection);
            _armadillo.Colliders.ActivateCollider(MyColliders.ColliderType.Main);
        }

        public override void Update()
        {
            if (!_canEnd) return;
        }

        private void CalculateNextDirection(Collision2D collision)
        {
            if (_canEnd && _intoBallModel.CheckIfWillHitTheFloor.IsRaycastCollidingWithLayer)
            {
                _rigidBody2D.velocity = Vector2.zero;
                _armadillo.ChangeBehaviour(Behaviour.Idle);
            }

            float speed = _currentSpeed.magnitude;
            Vector2 direction = Vector2.Reflect(_currentSpeed.normalized, collision.contacts[0].normal);
            _currentSpeed = direction * speed;
            _rigidBody2D.velocity = _currentSpeed;
        }

        private IEnumerator TurnIntoBall()
        {
            _mainAnimator.PlayAnimation(ArmadilloAnimatorModel.AnimationName.INTO_BALL);

            yield return new WaitForSeconds(_intoBallModel.CdwToStartBouncing);

            _armadillo.Colliders.ActivateCollider(MyColliders.ColliderType.Ball);
            _basicBallCollider.OnCollision.AddListener(CalculateNextDirection);

            Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), 1f);

            _currentSpeed = direction * _intoBallModel.Velocity;

            _rigidBody2D.velocity = _currentSpeed;

            _armadillo.StartCoroutine(CalculateDuration());
        }

        private IEnumerator CalculateDuration()
        {
            yield return new WaitForSeconds(_intoBallModel.Duration);
            _canEnd = true;
        }
    }
}