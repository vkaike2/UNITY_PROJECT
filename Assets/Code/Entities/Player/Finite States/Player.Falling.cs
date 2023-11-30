using System;
using System.Collections;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    private class Falling : PlayerBaseState
    {
        public override FiniteState State => FiniteState.Falling;

        public override bool ImFistState()
        {
            return !IsPlayerTouchingGround;
        }

        public override void EnterState()
        {
            AssignEvents();

            _animator.PlayAnimation(PlayerAnimatorModel.Animation.Falling);
            _rigidbody2D.gravityScale = _fallingModel.GravityFalling;
        }

        public override void OnExitState()
        {
            UnassignEvents();

            _rigidbody2D.gravityScale = _initialGravity;
        }

        public override void Update()
        {
            if (CheckIfPlayerIsOnTheGround()) return;

            MovePlayerInTheAir();
        }

        private void AssignEvents()
        {
            _player.JumpInput.Performed.AddListener(OnJumpInputPerformed);

            ManagePoopPerformedEvent(true);
        }

        private void UnassignEvents()
        {
            _player.JumpInput.Performed.RemoveListener(OnJumpInputPerformed);

            ManagePoopPerformedEvent(false);
        }

        //It is called for coyote time
        private void OnJumpInputPerformed()
        {
            if (IsBufferJumpTouchingGround())
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
                ChangeState(FiniteState.Jump, ignoreValidations: true);
                return;
            }

            if (_jumpModel.IsCoyoteTimeActive)
            {
                ChangeState(FiniteState.Jump, ignoreValidations: true);
            }
        }

        private bool IsBufferJumpTouchingGround() => _jumpModel.BufferCheck.DrawPhysics2D(_jumpModel.GroundLayer) != null;

        private bool CheckIfPlayerIsOnTheGround()
        {
            if (!IsPlayerTouchingGround) return false;
            if (_jumpModel.IsBeingControlledByKnockback) return false;            

            if (_player.MoveInput.Value == Vector2.zero)
            {
                CheckIfPlayerIsLandingOnAPlatform();

                ChangeState(FiniteState.Idle);
                return true;
            }

            return ChangeState(FiniteState.Move);
        }

        private void CheckIfPlayerIsLandingOnAPlatform()
        {
            Collider2D collider = _jumpModel.GroundCheck.DrawPhysics2D(_jumpModel.GroundLayer);
            if (collider.GetComponent<OneWayPlatform>() != null)
            {
                _jumpModel.IsLandingOnAPlatform = true;
            }
        }
    }
}