using System;
using System.Collections;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    private class Idle : PlayerBaseState
    {
        public override FiniteState State => FiniteState.Idle;

        public override bool ImFistState()
        {
            return IsPlayerTouchingGround;
        }

        public override void EnterState()
        {
            AssignEvents();

            _animator.PlayAnimation(PlayerAnimatorModel.Animation.Idle);

            _jumpModel.IsBeingControlledByKnockback = false;

            if (!_jumpModel.IsLandingOnAPlatform)
            {
                FreezeRigidbodyConstraints(true);
            }
            else
            {
                _jumpModel.IsLandingOnAPlatform = false;
            }
        }

        public override void OnExitState()
        {
            UnassignEvents();

            FreezeRigidbodyConstraints(false);
        }

        public override void Update()
        {
            if (CheckIfPlayerIsFalling()) return;
        }

        private bool CheckIfPlayerIsFalling()
        {
            if (_jumpModel.IsLandingOnAPlatform) return false;
            if (IsPlayerTouchingGround) return false;

            return ChangeState(FiniteState.Falling);
        }

        private void AssignEvents()
        {
            _player.MoveInput.Performed.AddListener(OnMoveInputPerformed);
            _player.JumpInput.Performed.AddListener(OnJumpInputPerformed);

            _player.DownPlatformInput.Performed.AddListener(DownPlatform);

            _fart.OnFartEvent.AddListener(GivePlayerControll);
            _player.DamageReceiver.OnKnockbackEvent.AddListener(GivePlayerControll);
            _damageReceiver.OnKnockbackEvent.AddListener(GivePlayerControll);

            ManagePooopPerformedEvent(true);
        }

        private void UnassignEvents()
        {
            _player.MoveInput.Performed.RemoveListener(OnMoveInputPerformed);
            _player.JumpInput.Performed.RemoveListener(OnJumpInputPerformed);

            _player.DownPlatformInput.Performed.RemoveListener(DownPlatform);

            _fart.OnFartEvent.RemoveListener(GivePlayerControll);
            _player.DamageReceiver.OnKnockbackEvent.RemoveListener(GivePlayerControll);
            _damageReceiver.OnKnockbackEvent.RemoveListener(GivePlayerControll);

            ManagePooopPerformedEvent(false);
        }

        private void GivePlayerControll(float seconds)
        {
            _player.StartCoroutine(GiveControll(seconds));
        }

        private void OnJumpInputPerformed()
        {
            ChangeState(FiniteState.Jump);
        }

        private void OnMoveInputPerformed()
        {
            ChangeState(FiniteState.Move);
        }

        private void FreezeRigidbodyConstraints(bool freeze)
        {
            if (freeze)
            {
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }

        private IEnumerator GiveControll(float seconds)
        {
            _jumpModel.IsBeingControlledByKnockback = true;
            FreezeRigidbodyConstraints(false);
            yield return new WaitForSeconds(seconds);

            if (_player.CurrentState == State)
            {
                FreezeRigidbodyConstraints(true);
            }
            _jumpModel.IsBeingControlledByKnockback = false;
        }

        private void DownPlatform()
        {
            if (!CanGoDownPlatform()) return;

            if (_downPlatformModel.PlayerCollider.enabled)
            {
                FreezeRigidbodyConstraints(false);
                _player.StartCoroutine(DeactivateColliderFor());
            }

            ChangeState(Player.FiniteState.Falling);
        }
    }
}
