using System;
using System.Collections;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override Player.State State => Player.State.Jump;

    private bool _isNotTouchingTheGround;
    private bool _isJumping = false;
    public override void EnterState()
    {
        _initialGravity = _rigidbody2D.gravityScale;

        _player.Animator.PlayAnimation(PlayerAnimatorModel.Animations.Jump);

        _player.MoveInput.Performed = () => OnMoveInputPerformed();
        _player.MoveInput.Canceled = () => OnMoveInputCanceled();

        _player.JumpInput.Performed = () => OnJumpInputPerformed();
        _isJumping = false;

        if (_player.PreviousState == Player.State.Falling)
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

        _rigidbody2D.velocity = new Vector2(_player.MoveInput.Value.x * _moveModel.MovementSpeed, _rigidbody2D.velocity.y);
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
            _player.ChangeState(Player.State.Falling);
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


            cdw += Time.deltaTime;
            actualJumpForce -= _jumpModel.CdwJumpAceleration * (cdw / _jumpModel.CdwJumpAceleration);
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, actualJumpForce);
            yield return new WaitForFixedUpdate();
        }

        _rigidbody2D.gravityScale = _jumpModel.GravityFalling;
    }
}
