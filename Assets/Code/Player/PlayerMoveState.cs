using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveState : PlayerBaseState
{
    public override Player.State State => Player.State.Move;

    public override void EnterState()
    {
        _player.Animator.PlayAnimation(PlayerAnimatorModel.Animation.Move);

        _player.MoveInput.Performed = () => OnMoveInputPerformed();
        _player.MoveInput.Canceled = () => OnMoveInputCanceled();

        _player.JumpInput.Started = () => OnJumpInputStarted();
    }

    public override bool ImFistState()
    {
        return _player.IsOnTheGround() && _player.MoveInput.Value.x != 0;
    }

    public override void Update()
    {
        CheckIfIsFalling();

        MovePlayerHorizontally();
    }


    protected override void OnMoveInputCanceled()
    {
        base.OnMoveInputCanceled();
        _player.StartCoroutine(WaitThenChangeToIdle());
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

    /// <summary>
    ///     check if you want to change to idle or if you are just changing directions
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitThenChangeToIdle()
    {
        yield return new WaitForSeconds(0.1f);

        if (_player.MoveInput.Value == Vector2.zero)
        {
            _player.ChangeState(Player.State.Idle);
        }
    }
}
