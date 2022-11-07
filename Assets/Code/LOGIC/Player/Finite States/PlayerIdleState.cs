using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerFiniteBaseState
{
    public override Player.FiniteState State => Player.FiniteState.Idle;

    public override void EnterState()
    {
        _player.Animator.PlayAnimation(PlayerAnimatorModel.Animation.Idle);

        _player.MoveInput.Started = () => OnMoveInputStarted();
        _player.JumpInput.Started = () => OnJumpInputStarted();

        _player.DownPlatform.Performed = () => OnDownPlatform();

        _player.PoopInput.Started = () => OnPoopInputStarted();
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
        _player.ChangeState(Player.FiniteState.Move);
    }

    protected override void OnJumpInputStarted()
    {
        base.OnJumpInputStarted();
        _player.ChangeState(Player.FiniteState.Jump);
    }

    private void OnDownPlatform()
    {
        DownPlatform();
    }

    private void CheckIfIsFalling()
    {
        if (!_player.IsOnTheGround())
        {
            _player.ChangeState(Player.FiniteState.Falling);
        }
    }

}

