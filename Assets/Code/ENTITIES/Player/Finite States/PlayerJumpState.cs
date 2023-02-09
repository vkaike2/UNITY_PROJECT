using System;
using System.Collections;
using UnityEngine;

public class PlayerJumpState : PlayerFiniteBaseState
{
    public override Player.FiniteState State => Player.FiniteState.Jump;

    private bool _isNotTouchingTheGround;
    private bool _isJumping = false;

    public override void EnterState()
    {
        _initialGravity = _rigidbody2D.gravityScale;

        _player.Animator.PlayAnimation(PlayerAnimatorModel.Animation.Jump);

        _player.MoveInput.Performed = () => OnMoveInputPerformed();
        _player.MoveInput.Canceled = () => OnMoveInputCanceled();

        _player.JumpInput.Performed = () => OnJumpInputPerformed();

        _player.PoopInput.Started = () => OnPoopInputStarted();

        _isJumping = false;

        if (_player.PreviousState == Player.FiniteState.Falling)
        {
            _player.StartCoroutine(Jump());
            _isNotTouchingTheGround = true;
        }
    }

    public override void Start(Player player)
    {
        base.Start(player);
        _jumpModel = _player.JumpStateModel;
    }

    public override void Update()
    {
        CheckIfPlayerStillJumping();

        MovePlayerHorizontally();
    }

    protected override void OnJumpInputPerformed()
    {
        base.OnJumpInputPerformed();
        if (_isJumping) return;
        _player.StartCoroutine(Jump());
        _player.StartCoroutine(MakeSurePlayerLeftTheGround());
    }

    private void CheckIfPlayerStillJumping()
    {
        if (!_isNotTouchingTheGround) return;

        if (_rigidbody2D.velocity.y <= 0)
        {
            _player.ChangeState(Player.FiniteState.Falling);
        }
    }

    IEnumerator MakeSurePlayerLeftTheGround()
    {
        _isNotTouchingTheGround = false;
        while (_player.IsOnTheGround())
        {
            yield return new WaitForFixedUpdate();
        }
        _isNotTouchingTheGround = true;
    }

    IEnumerator Jump()
    {
        _isJumping = true;
        float cdw = 0;
        float actualJumpForce = _jumpModel.JumpForce;
        while (cdw <= _jumpModel.CdwJumpAceleration)
        {
            if (!_player.JumpInput.Value)
            {
                break;
            }

            if (_player.FartInput.Value)
            {
                break;
            }

            cdw += Time.deltaTime;
            actualJumpForce -= _jumpModel.CdwJumpAceleration * (cdw / _jumpModel.CdwJumpAceleration);
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, actualJumpForce);
            yield return new WaitForFixedUpdate();
        }

        _rigidbody2D.gravityScale = _jumpModel.GravityFalling;
    }
}
