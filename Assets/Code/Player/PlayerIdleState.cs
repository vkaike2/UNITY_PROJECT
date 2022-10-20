using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerBaseState
{
    public override Player.State State => Player.State.Idle;

    public override void EnterState()
    {
        _player.Animator.PlayAnimation(PlayerAnimatorModel.Animation.Idle);

        _player.MoveInput.Started = () => OnMoveInputStarted();
        _player.JumpInput.Started = () => OnJumpInputStarted();
    }

    public override bool ImFistState()
    {
        return _player.IsOnTheGround() && _player.MoveInput.Value == Vector2.zero;
    }

    public override void Update()
    {
        CheckIfIsFalling();
        MovePlayerHorizontally();
    }

    protected override void OnMoveInputStarted()
    {
        base.OnMoveInputStarted();
        _player.ChangeState(Player.State.Move);
    }

    protected override void OnJumpInputStarted()
    {
        base.OnJumpInputStarted();
        _player.ChangeState(Player.State.Jump);
    }

    private void CheckIfIsFalling()
    {
        if (!_player.IsOnTheGround())
        {
            _player.ChangeState(Player.State.Falling);
        }
    }
}

