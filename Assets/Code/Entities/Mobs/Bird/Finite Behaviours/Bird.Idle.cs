using System;
using System.Collections;
using UnityEngine;


public partial class Bird : Enemy
{
    private class Idle : BirdBaseBehaviour
    {
        public override Bird.Behaviour Behaviour => Bird.Behaviour.Idle;

        private BirdIdleModel _model;
        private bool _isFlyingBackUp = false;
        private bool _isIdleOnTheSky = false;
        public override void Start(Enemy enemy)
        {
            base.Start(enemy);

            _model = _bird.IdleModel;
        }

        public override void OnEnterBehaviour()
        {
            _isFlyingBackUp = false;
            _bird.PatrolModel.OnFlappingWings.AddListener(OnFlappingWings);
            HandleIdleMovement();
        }


        public override void OnExitBehaviour()
        {
            _bird.PatrolModel.OnFlappingWings.RemoveListener(OnFlappingWings);
            EnableColliers(false);
        }

        public override void Update()
        {
        }

        private void HandleIdleMovement()
        {
            if (_model.GroundCheck.IsRaycastCollidingWithLayer)
            {
                _bird.StartCoroutine(IsIdlingOnTheGround());
                return;
            }

            _bird.StartCoroutine(IsIdlingOnTeSky());
        }

        private IEnumerator IsIdlingOnTeSky()
        {
            _bird.BirdAnimator.PlayAnimation(BridAnimatorModel.AnimationName.Bird_Fly);

            _isIdleOnTheSky = true;
            yield return new WaitForSeconds(_model.CdwToFlyBack);
            _isIdleOnTheSky = false;

            _bird.StartCoroutine(BirdGoingBackUp());
        }

        private IEnumerator IsIdlingOnTheGround()
        {
            EnableColliers(true);
            _bird.BirdAnimator.PlayAnimation(BridAnimatorModel.AnimationName.Brid_Ground_Idle);

            yield return new WaitForSeconds(_model.CdwToFlyBack);

            EnableColliers(false);
            _bird.StartCoroutine(BirdGoingBackUp());
        }

        private void EnableColliers(bool isEnabled)
        {
            foreach (var collider in _model.MainColliders)
            {
                collider.enabled = isEnabled;
            }
        }

        private IEnumerator BirdGoingBackUp()
        {
            _isFlyingBackUp = true;
            _bird.BirdAnimator.PlayAnimation(BridAnimatorModel.AnimationName.Bird_Fly);

            Vector2 myHorizontalPosition = new Vector2(0, _bird.transform.position.y);
            Vector2 initialPosition = new Vector2(0, _bird.PatrolModel.InitialHorizontalPosition);

            while (Vector2.Distance(myHorizontalPosition, initialPosition) >= 0.2f)
            {
                myHorizontalPosition = new Vector2(0, _bird.transform.position.y);
                yield return new WaitForFixedUpdate();
            }

            _isFlyingBackUp = false;
            _bird.ChangeBehaviour(Behaviour.Patrol);
        }

        private void OnFlappingWings()
        {
            if (_isIdleOnTheSky)
            {
                _rigidbody2D.velocity = new Vector2(0, _bird.PatrolModel.HorizontalVelocity);
                return;
            }

            if (_isFlyingBackUp)
            {
                _rigidbody2D.velocity = new Vector2(0, _bird.AtkModel.MovementSpeedToGoUp);
            }
        }

    }
}
