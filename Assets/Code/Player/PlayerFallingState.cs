using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFallingState : PlayerBaseState
{
    public override Player.State State => Player.State.Falling;
    private bool _triedToJump = false;

    public override void EnterState()
    {
        _player.Animator.PlayAnimation(PlayerAnimatorModel.Animation.Falling);

        _rigidbody2D.gravityScale = _jumpModel.GravityFalling;

        _player.MoveInput.Performed = () => OnMoveInputPerformed();
        _player.MoveInput.Canceled = () => OnMoveInputCanceled();

        _player.JumpInput.Performed = () => OnJumpInputPerformed();
        
        _triedToJump = false;

        if (_player.PreviousState != null && _player.PreviousState == Player.State.Move)
        {
            _player.StartCoroutine(CheckForCoyoteTime());
        }
    }

    public override bool ImFistState()
    {
        return !_player.IsOnTheGround();
    }

    public override void Update()
    {
        MovePlayerHorizontally();

        CheckIfStillFalling();
    }

    protected override void OnJumpInputPerformed()
    {
        base.OnJumpInputPerformed();
        _triedToJump = true;
        if (IsOnBufferDistanceToJump())
        {
            ChangeStateFixGravity(Player.State.Jump);
        }
    }

    private void ChangeStateFixGravity(Player.State state)
    {
        _rigidbody2D.gravityScale = _initialGravity;
        _player.ChangeState(state);
    }

    private void CheckIfStillFalling()
    {
        if (!_player.IsOnTheGround()) return;

        if (_player.MoveInput.Value.x == 0)
        {
            ChangeStateFixGravity(Player.State.Idle);
        }
        else
        {
            ChangeStateFixGravity(Player.State.Move);
        }
    }

    IEnumerator CheckForCoyoteTime()
    {
        float cdw = 0;
        while (cdw <= _jumpModel.CoyoteTime)
        {
            cdw += Time.deltaTime;
            yield return new WaitForFixedUpdate();

            if (_triedToJump)
            {
                _player.ChangeState(Player.State.Jump);
            }
        }
    }

    private bool IsOnBufferDistanceToJump()
    {
        Collider2D col = _jumpModel.BufferCheck.DrawPhysics2D(_jumpModel.GroundLayer);
        return col != null;
    }
}
