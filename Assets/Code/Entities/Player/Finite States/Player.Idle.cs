using System;
using System.Collections;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    private class Idle : PlayerBaseState
    {
        //private bool _knockBackBuffer;

        public override FiniteState State => FiniteState.Idle;

        public override void Start(Player player)
        {
            base.Start(player);
            _player.OnKnockbackEvent.AddListener(GivePlayerControl);
        }

        public override bool ImFistState()
        {
            return IsPlayerTouchingGround;
        }

        public override void EnterState()
        {
            AssignEvents();

            _animator.PlayAnimation(PlayerAnimatorModel.Animation.Idle);

            if (!_jumpModel.IsLandingOnAPlatform)
            {
                FreezeRigidBodyConstraints(true);
            }
            else
            {
                _jumpModel.IsLandingOnAPlatform = false;
            }
        }

        public override void OnExitState()
        {
            UnassignEvents();

            FreezeRigidBodyConstraints(false);
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

            ManagePoopPerformedEvent(true);
        }

        private void UnassignEvents()
        {
            _player.MoveInput.Performed.RemoveListener(OnMoveInputPerformed);
            _player.JumpInput.Performed.RemoveListener(OnJumpInputPerformed);

            _player.DownPlatformInput.Performed.RemoveListener(DownPlatform);

            ManagePoopPerformedEvent(false);
        }

        private void GivePlayerControl(float seconds, KnockBackSource source)
        {

            //if (source == KnockBackSource.Fart)
            //{
            //    return;
            //}

            _player.StartCoroutine(GiveControl(seconds));
        }

        private void OnJumpInputPerformed()
        {
            ChangeState(FiniteState.Jump);
        }

        private void OnMoveInputPerformed()
        {
            ChangeState(FiniteState.Move);
        }

        private void FreezeRigidBodyConstraints(bool freeze)
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

        private IEnumerator GiveControl(float seconds)
        {
            //if (_player.CurrentState != State)
            //{
            //    _player.StartCoroutine(WaitForKnockbackBuffer(seconds));
            //}

            FreezeRigidBodyConstraints(false);
            yield return new WaitForSeconds(seconds);

            if (_player.CurrentState == State)
            {
                FreezeRigidBodyConstraints(true);
            }
        }

        //private IEnumerator WaitForKnockbackBuffer(float knockBackSeconds)
        //{
        //    _knockBackBuffer = true;
        //    yield return new WaitForSeconds(knockBackSeconds);
        //    _knockBackBuffer = false;
        //}

        private void DownPlatform()
        {
            if (!CanGoDownPlatform()) return;

            if (_downPlatformModel.PlayerCollider.enabled)
            {
                FreezeRigidBodyConstraints(false);
                _player.StartCoroutine(DeactivateColliderFor());
            }

            ChangeState(Player.FiniteState.Falling);
        }
    }
}
