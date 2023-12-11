using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


public partial class GunGaroo : MonoBehaviour
{
    private class JumpToOtherSide : GunGarooBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.JumpToOtherSide;

        private GunGarooContainer _container;
        private GunGarooJumpModel _jumpModel;

        private List<Vector2> _jumpPositions;
        private Vector2 _currentTargetPosition = Vector2.zero;
        private bool _isLastJump = false;

        private Vector3 _originalRotation;

        private Coroutine _startJumpProcess;
        private Coroutine _manageJump;

        public override void Start(GunGaroo gunGaroo)
        {
            base.Start(gunGaroo);
            _container = gunGaroo.container;
            _jumpModel = gunGaroo.JumpModel;

            _jumpPositions = Get4JumpPositions();
        }

        public override void OnEnterBehaviour()
        {
            _originalRotation = _gunGaroo.rotationalTransform.localScale;
            _startJumpProcess = _gunGaroo.StartCoroutine(StartJumpProcess());
        }

        public override void OnExitBehaviour()
        {
            _currentTargetPosition = Vector2.zero;
            _isLastJump = false;

            if (_startJumpProcess != null) _gunGaroo.StopCoroutine(_startJumpProcess);
            if (_manageJump != null) _gunGaroo.StopCoroutine(_manageJump);
        }

        public override void Update()
        {
        }

        private IEnumerator StartJumpProcess()
        {
            if (_isLastJump)
            {
                _gunGaroo.rotationalTransform.localScale = _originalRotation;
                _gunGaroo.ChangeBehaviour(Behaviour.Idle);

                yield break;
            }

            _gunGaroo.MainAnimator.PlayAnimation(GunGarooAnimatorModel.AnimationName.Jump_Start);

            List<float> jumpAngles = new List<float>() { 70 };

            GetNextJumpPosition();
            if (_currentTargetPosition == _jumpPositions[0])
            {
                jumpAngles = null;
            }

            GunGarooLandingVFX landingVfx = _gunGaroo.SpawnLandingVFX(_currentTargetPosition);

            Vector2 jumpForce = MovementUtils.CalculateJumpVelocity(
                _currentTargetPosition,
                _gunGaroo.transform.position,
                customAngles: jumpAngles);

            _gunGaroo.FlipGunGaroo(jumpForce.x > _gunGaroo.transform.position.x);

            yield return new WaitForSeconds(_jumpModel.CdwBetweenEachJump);


            _rigidBody2D.velocity = jumpForce;
            _manageJump = _gunGaroo.StartCoroutine(ManageJump(landingVfx));
        }

        private IEnumerator ManageJump(GunGarooLandingVFX landingVfx)
        {
            yield return new WaitUntil(() => !_gunGaroo.GroundCheck.IsRaycastCollidingWithLayer);

            while (!_gunGaroo.GroundCheck.IsRaycastCollidingWithLayer)
            {
                if (_rigidBody2D.velocity.y > 0)
                {
                    _gunGaroo.MainAnimator.PlayAnimation(GunGarooAnimatorModel.AnimationName.Jump_Air);
                }
                else
                {
                    _gunGaroo.MainAnimator.PlayAnimation(GunGarooAnimatorModel.AnimationName.Jump_Air_Down);
                }

                yield return new WaitForFixedUpdate();
            }

            Destroy(landingVfx.gameObject);
            _startJumpProcess = _gunGaroo.StartCoroutine(StartJumpProcess());
            _rigidBody2D.velocity = Vector2.zero;
        }

        private void GetNextJumpPosition()
        {
            if (_currentTargetPosition == Vector2.zero)
            {
                _currentTargetPosition = _jumpPositions[1];
                return;
            }


            if (_currentTargetPosition == _jumpPositions[1])
            {
                _currentTargetPosition = _jumpPositions[2];
                return;
            }

            if (_currentTargetPosition == _jumpPositions[2])
            {
                _currentTargetPosition = _jumpPositions[3];
                return;
            }

            if (_currentTargetPosition == _jumpPositions[3])
            {
                _currentTargetPosition = _jumpPositions[4];
                return;
            }

            if (_currentTargetPosition == _jumpPositions[4])
            {
                _isLastJump = true;
                _currentTargetPosition = _jumpPositions[0];
                return;
            }
        }

        private List<Vector2> Get4JumpPositions()
        {
            List<Vector2> jumpPositions = new List<Vector2>();

            jumpPositions.Add(_container.StartJumpPosition.position);

            Vector2 horizontalFirstPosition = new Vector2(_container.StartJumpPosition.position.x, 0);
            Vector2 horizontalEndPosition = new Vector2(_container.EndJumpPosition.position.x, 0);

            float deltaTotalDistance = Vector2.Distance(horizontalEndPosition, horizontalFirstPosition);
            float deltaPartialDistance = deltaTotalDistance / 4f;

            bool isJumpingToRightDirection = horizontalFirstPosition.x < horizontalEndPosition.y;

            jumpPositions.Add(
                new Vector2(
                    isJumpingToRightDirection ? _container.StartJumpPosition.position.x + deltaPartialDistance : _container.StartJumpPosition.position.x - deltaPartialDistance,
                    _container.StartJumpPosition.position.y));

            jumpPositions.Add(
                new Vector2(
                    isJumpingToRightDirection ? _container.StartJumpPosition.position.x + (deltaPartialDistance * 2) : _container.StartJumpPosition.position.x - (deltaPartialDistance * 2),
                    _container.StartJumpPosition.position.y));

            jumpPositions.Add(
                new Vector2(
                    isJumpingToRightDirection ? _container.StartJumpPosition.position.x + (deltaPartialDistance * 3) : _container.StartJumpPosition.position.x - (deltaPartialDistance * 3),
                    _container.StartJumpPosition.position.y));


            jumpPositions.Add(_container.EndJumpPosition.position);

            return jumpPositions;
        }

      
    }
}
