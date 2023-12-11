using Cinemachine.Utility;
using System.Collections;
using TMPro;
using UnityEngine;

public partial class GunGaroo : MonoBehaviour
{
    private class SuperJump : GunGarooBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.SuperJump;

        private GunGarooContainer _container;
        private GunGarooJumpModel _jumpModel;
        private Vector3 _originalRotation;


        private Coroutine _startSuperJumpProcess;
        private Coroutine _jumpBackToInitialPosition;
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
            _startSuperJumpProcess =_gunGaroo.StartCoroutine(StartSuperJumpProcess());
        }

        public override void OnExitBehaviour()
        {
            if(_startSuperJumpProcess != null) _gunGaroo.StopCoroutine(_startSuperJumpProcess);
            if(_jumpBackToInitialPosition != null) _gunGaroo.StopCoroutine(_jumpBackToInitialPosition);
            if(_manageJump != null) _gunGaroo.StopCoroutine(_manageJump);
        }

        public override void Update()
        {
        }

        private IEnumerator StartSuperJumpProcess()
        {
            _gunGaroo.MainAnimator.PlayAnimation(GunGarooAnimatorModel.AnimationName.SuperJump_Start);
            yield return new WaitForSeconds(_jumpModel.CdwBetweenEachJump);

            _gunGaroo.MainAnimator.PlayAnimation(GunGarooAnimatorModel.AnimationName.SuperJump_Air_Up);

            // Go up
            while (_gunGaroo.transform.position.y < _container.MaxHeightPosition.position.y)
            {
                _rigidBody2D.velocity = new Vector2(0, _jumpModel.SuperJumpSpeed);
                yield return new WaitForFixedUpdate();
            }
            _gunGaroo.FreezeRigidBodyConstraints(true);
            yield return new WaitForSeconds(_jumpModel.CdwToSuperJumpGoDown.GetRandom());

            // Move to player position
            _gunGaroo.MainAnimator.PlayAnimation(GunGarooAnimatorModel.AnimationName.SuperJump_Air_Down);
            Vector2 playerPosition = _gunGaroo.gameManager.Player.transform.position;
            _gunGaroo.transform.position = new Vector2(playerPosition.x, _gunGaroo.transform.position.y);

            GunGarooLandingVFX landingVfx = _gunGaroo.SpawnLandingVFX(new Vector2(playerPosition.x, _container.StartJumpPosition.position.y));

            // Come down
            _gunGaroo.FreezeRigidBodyConstraints(false);
            while (!_gunGaroo.GroundCheck.IsRaycastCollidingWithLayer)
            {
                _rigidBody2D.velocity = new Vector2(0, -_jumpModel.SuperJumpSpeed);
                yield return new WaitForFixedUpdate();
            }

            // Landing
            Destroy(landingVfx.gameObject);
            _rigidBody2D.velocity = Vector2.zero;
            _gunGaroo.MainAnimator.PlayAnimation(GunGarooAnimatorModel.AnimationName.SuperJump_Landing);

            yield return new WaitForSeconds(_jumpModel.CdwBetweenEachJump);

            _jumpBackToInitialPosition =_gunGaroo.StartCoroutine(JumpBackToInitialPosition());
        }

        private IEnumerator JumpBackToInitialPosition()
        {
            _gunGaroo.MainAnimator.PlayAnimation(GunGarooAnimatorModel.AnimationName.Jump_Start);

            Vector2 targetPosition = _container.StartJumpPosition.position;

            Vector2 jumpForce = MovementUtils.CalculateJumpVelocity(
                 targetPosition,
                _gunGaroo.transform.position);

            GunGarooLandingVFX landingVfx = _gunGaroo.SpawnLandingVFX(targetPosition);
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

            _rigidBody2D.velocity = Vector2.zero;

            Destroy(landingVfx.gameObject);
            _gunGaroo.rotationalTransform.localScale = _originalRotation;
            _gunGaroo.ChangeBehaviour(Behaviour.Idle);
        }
    }
}
