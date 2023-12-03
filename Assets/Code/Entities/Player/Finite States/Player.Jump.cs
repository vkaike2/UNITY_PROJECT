using Cinemachine.Utility;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    private class Jump : PlayerBaseState
    {
        public override FiniteState State => FiniteState.Jump;

        private Coroutine _jumpPerformedCoroutine;

        /// <summary>
        ///     wait until you left the ground with your jump
        /// </summary>
        private bool _hasLeftTheGround = false;

        public override void EnterState()
        {
            AssignEvents();
            _animator.PlayAnimation(PlayerAnimatorModel.Animation.Jump);

            _player.StartCoroutine(MakeSurePlayerLeftTheGround());
            _jumpPerformedCoroutine = _player.StartCoroutine(PerformJump());
        }

        public override void OnExitState()
        {
            UnassignEvents();
            StopJumping();
        }

        public override void Update()
        {
            if (CheckIfIsFalling() || CheckIfTouchedGround()) return;

            MovePlayerInTheAir();
        }

        private void AssignEvents()
        {
            _player.JumpInput.Canceled.AddListener(OnJumpInputCanceled);
            _player.FartInput.Canceled.AddListener(OnFartInputCanceled);

            ManagePoopPerformedEvent(true);
        }

        private void UnassignEvents()
        {
            _player.JumpInput.Canceled.RemoveListener(OnJumpInputCanceled);
            _player.FartInput.Canceled.RemoveListener(OnFartInputCanceled);

            ManagePoopPerformedEvent(false);
        }

        private void OnJumpInputCanceled()
        {
            StopJumping();
        }

        private void OnFartInputCanceled()
        {
            StopJumping();
        }

        private void StopJumping()
        {
            if (_jumpPerformedCoroutine == null) return;
            _player.StopCoroutine(_jumpPerformedCoroutine);
        }

        private bool CheckIfIsFalling()
        {
            if (_rigidbody2D.velocity.y > 0) return false;
            return ChangeState(FiniteState.Falling);
        }

        private bool CheckIfTouchedGround()
        {
            if (!_hasLeftTheGround) return false;
            if (!IsPlayerTouchingGround) return false;
            if (_jumpModel.GroundCheck.DrawPhysics2D(_jumpModel.GroundLayer).gameObject.GetComponent<OneWayPlatform>() != null) return false;

            if (_player.MoveInput.Value == Vector2.zero && !_player.KnockBackInfo.IsBeingControlledByKnockBack)
            {
                return ChangeState(FiniteState.Idle);
            }

            return ChangeState(FiniteState.Move);
        }

        private IEnumerator MakeSurePlayerLeftTheGround()
        {
            _hasLeftTheGround = false;
            while (IsPlayerTouchingGround)
            {
                yield return new WaitForFixedUpdate();
            }
            _hasLeftTheGround = true;
        }

        private IEnumerator PerformJump()
        {
            float cdw = 0;
            float actualJumpForce = _status.JumpForce.Get();
            while (cdw <= _jumpModel.CdwJumpAceleration)
            {

                if (_player.KnockBackInfo.IsBeingControlledByKnockBack)
                {
                    break;
                }

                cdw += Time.deltaTime;
                actualJumpForce -= _jumpModel.CdwJumpAceleration * (cdw / _jumpModel.CdwJumpAceleration);
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, actualJumpForce);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}