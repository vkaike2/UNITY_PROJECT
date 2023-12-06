using System;
using System.Collections;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    private class Move : PlayerBaseState
    {
        public override FiniteState State => FiniteState.Move;

        public override void EnterState()
        {
            AssignEvents();

            FlipPlayerToTheCorrectDirection();
            _animator.PlayAnimation(PlayerAnimatorModel.Animation.Move);
        }

        public override void OnExitState()
        {
            UnassignEvents();
        }

        public override void Update()
        {
            if (CheckIfPlayerIsFalling()) return;

            MovePlayerOnTheGround();
        }

        private bool CheckIfPlayerIsFalling()
        {
            if (IsPlayerTouchingGround) return false;

            _player.StartCoroutine(CalculateCoyoteTime());
            return ChangeState(FiniteState.Falling);
        }

        private void MovePlayerOnTheGround()
        {
            if (_player.KnockBackInfo.IsBeingControlledByKnockBack)
            {
                if (_player.KnockBackInfo.Source == KnockBackSource.Fart)
                {
                    _rigidBody2D.velocity = new Vector2(0, _rigidBody2D.velocity.y);
                }
                // Will do nothing if its a damage source knockback
                return;
            }

            _rigidBody2D.velocity = new Vector2(_player.MoveInput.Value.x * _status.MovementSpeed.Get(), _rigidBody2D.velocity.y);
        }

        private void AssignEvents()
        {
            _player.EatInput.Performed.AddListener(OnEatInputPerformed);
            _player.MoveInput.Canceled.AddListener(OnMoveInputCanceled);
            _player.JumpInput.Performed.AddListener(OnJumpInputPerformed);

            _player.DownPlatformInput.Performed.AddListener(DownPlatform);

            ManagePoopPerformedEvent(true);
        }

        private void UnassignEvents()
        {
            _player.EatInput.Performed.RemoveListener(OnEatInputPerformed);
            _player.MoveInput.Canceled.RemoveListener(OnMoveInputCanceled);
            _player.JumpInput.Performed.RemoveListener(OnJumpInputPerformed);

            _player.DownPlatformInput.Performed.RemoveListener(DownPlatform);

            ManagePoopPerformedEvent(false);
        }

        private void DownPlatform()
        {
            if (!CanGoDownPlatform()) return;

            if (_downPlatformModel.PlayerCollider.enabled)
            {
                _player.StartCoroutine(DeactivateColliderFor());
            }

            ChangeState(Player.FiniteState.Falling);
        }

        private void OnJumpInputPerformed()
        {
            ChangeState(FiniteState.Jump);
        }

        private void OnMoveInputCanceled()
        {
            ChangeState(FiniteState.Idle);
        }

        private IEnumerator CalculateCoyoteTime()
        {
            _jumpModel.IsCoyoteTimeActive = true;
            yield return new WaitForSeconds(_jumpModel.CoyoteTime);
            _jumpModel.IsCoyoteTimeActive = false;
        }
    }
}