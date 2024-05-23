using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Seagull : Enemy
{
    private class Fly : SeagullBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Fly;

        private const float TIME_TO_CHECK_IF_NEED_TO_FLIP = 1f;
        private bool _canChangeDirection = true;
        private Coroutine _attackCoroutine;

        private float _verticalVelocity;
        private bool _isLanding = false;
        private bool _isFirstVelocity = true;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _flyModel.InitialVerticalPosition = _seagull.transform.position.y;
        }

        public override void OnEnterBehaviour()
        {
            _seagull.IsFlying = true;
            _verticalVelocity = _flyModel.VerticalVelocity;
            _isLanding = false;
            _isFirstVelocity = true;

            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _flyModel.OnFlappingWings.AddListener(OnFlapWings);
            _seagull.Animator.PlayAnimation(SeagullAnimatorModel.AnimationName.Seagull_Fly);
            _attackCoroutine = _seagull.StartCoroutine(ManageAttack());
            _seagull.StartCoroutine(ManageFlightDuration());
        }

        public override void OnExitBehaviour()
        {
            _flyModel.OnFlappingWings.RemoveListener(OnFlapWings);
            _seagull.StopCoroutine(_attackCoroutine);
        }


        public override void Update()
        {
            HorizontalMovement();
            FlipIfNecessary();
        }

        #region MOVEMENT

        private void OnFlapWings()
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _verticalVelocity);
        }

        private void HorizontalMovement()
        {
            if (_isLanding) return;

            float velocity = GetHorizontalVelocity();
            if (_seagull.RotationalTransform.localScale.x == -1)
            {
                velocity *= -1;
            }

            _rigidbody2D.velocity = new Vector2(velocity, _rigidbody2D.velocity.y);
        }

        private float GetHorizontalVelocity()
        {
            if (_isFirstVelocity)
            {
                _isFirstVelocity = false;
                return GetRandomInitialDirection() * _seagull.Status.MovementSpeed.Get();
            }
            return _seagull.Status.MovementSpeed.Get();
        }

        private float GetRandomInitialDirection()
        {
            List<float> allPossibilites = new List<float>() { -1, 1 };

            return allPossibilites[UnityEngine.Random.Range(0, allPossibilites.Count)];
        }

        private void FlipIfNecessary()
        {
            if (!_canChangeDirection) return;
            if (_seagull.SkySection.ImInsideSkyLimit(_seagull.transform.position)) return;

            _seagull.RotationalTransform.localScale = new Vector3(
                _seagull.RotationalTransform.localScale.x * -1,
                _seagull.RotationalTransform.localScale.y,
                _seagull.RotationalTransform.localScale.z);

            _canChangeDirection = false;
            _seagull.StartCoroutine(AddCdwToAllowSeagullToChangeDirection());
        }

        private IEnumerator AddCdwToAllowSeagullToChangeDirection()
        {
            yield return new WaitForSeconds(TIME_TO_CHECK_IF_NEED_TO_FLIP);
            _canChangeDirection = true;
        }


        private IEnumerator ManageFlightDuration()
        {
            yield return new WaitForSeconds(_flyModel.FlyDuration);

            yield return new WaitUntil(() => _flyModel.CheckIfImAboveGround.IsRaycastCollidingWithLayer);
            _rigidbody2D.velocity = Vector2.zero;
            _isLanding = true;
            _verticalVelocity /= 2;

            yield return new WaitUntil(() => _flyModel.CheckIsTouchingGround.IsRaycastCollidingWithLayer);
            _seagull.ChangeBehaviour(Behaviour.Ground);
        }
        #endregion

        #region ATTACK

        private IEnumerator ManageAttack()
        {
            yield return new WaitForSeconds(_flyModel.CdwAttack);
            DropPoop();

            _attackCoroutine = _seagull.StartCoroutine(ManageAttack());
        }

        private void DropPoop()
        {
            SeagullProjectile projectile = GameObject.Instantiate(_flyModel.SeagullProjectilePrefab, _flyModel.PoopPosition.position, Quaternion.identity);
            projectile.transform.parent = null;
            _seagull.DamageDealer.OnRegisterProjectileEvent.Invoke(projectile);
        }

        #endregion
    }
}