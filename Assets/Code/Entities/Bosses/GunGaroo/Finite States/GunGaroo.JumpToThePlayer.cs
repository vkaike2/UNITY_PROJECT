using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GunGaroo : MonoBehaviour
{
    private class JumpToThePlayer : GunGarooBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.JumpToThePlayer;

        private GunGarooContainer _container;
        private GunGarooJumpModel _jumpModel;

        private Vector3 _originalRotation;

        private bool _isLastJump = false;
        private Coroutine _startJump;
        private Coroutine _manageJump;

        public override void Start(GunGaroo gunGaroo)
        {
            base.Start(gunGaroo);
            _container = gunGaroo.container;
            _jumpModel = gunGaroo.JumpModel;
        }

        public override void OnEnterBehaviour()
        {
            _originalRotation = _gunGaroo.rotationalTransform.localScale;
            _startJump = _gunGaroo.StartCoroutine(StartJump(true));
        }

        public override void OnExitBehaviour()
        {
            _isLastJump = false;

            if(_startJump != null) _gunGaroo.StopCoroutine(_startJump);
            if(_manageJump != null) _gunGaroo.StopCoroutine(_manageJump);
        }

        public override void Update()
        {
        }

        private IEnumerator StartJump(bool isJumpingTowardsPlayer)
        {
            _gunGaroo.MainAnimator.PlayAnimation(GunGarooAnimatorModel.AnimationName.Jump_Start);

            if (_isLastJump)
            {
                _gunGaroo.rotationalTransform.localScale = _originalRotation;
                _gunGaroo.ChangeBehaviour(Behaviour.Idle);
                yield break;
            }


            Vector2 targetPosition = _gunGaroo.gameManager.Player.transform.position;
                
            if (!isJumpingTowardsPlayer)
            {
                _isLastJump = true;

                targetPosition = _container.StartJumpPosition.position;
            }

            GunGarooLandingVFX landingVfx = _gunGaroo.SpawnLandingVFX(targetPosition);

            Vector2 jumpForce = MovementUtils.CalculateJumpVelocity(
                 targetPosition,
                _gunGaroo.transform.position);

            _gunGaroo.FlipGunGaroo(jumpForce.x > _gunGaroo.transform.position.x);


            yield return new WaitForSeconds(_jumpModel.CdwBetweenEachJump);


            _rigidBody2D.velocity = jumpForce;
            _manageJump =_gunGaroo.StartCoroutine(ManageJump(landingVfx));
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
            _startJump = _gunGaroo.StartCoroutine(StartJump(false));
            _rigidBody2D.velocity = Vector2.zero;
        }


    }
}
