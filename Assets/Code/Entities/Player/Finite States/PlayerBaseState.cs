using System.Collections;
using UnityEngine;

public abstract class PlayerBaseState
{
    protected Player _player;
    protected Rigidbody2D _rigidbody2D;
    protected Fart _fart;
    protected PlayerDamageReceiver _damageReceiver;

    protected PlayerStatus _status;
    protected PlayerAnimatorModel _animator;

    protected PlayerJumpModel _jumpModel;
    protected PlayerFallingModel _fallingModel;
    protected PlayerDownPlatformModel _downPlatformModel;
    protected PlayerPoopModel _poopModel;

    protected float _initialGravity;

    protected bool ImTheActiveState => _player.CurrentState == State;

    public abstract Player.FiniteState State { get; }

    public virtual bool ImFistState()
    {
        return false;
    }

    public virtual void Start(Player player)
    {
        _player = player;
        _rigidbody2D = _player.GetComponent<Rigidbody2D>();
        _fart = _player.GetComponent<Fart>();
        _damageReceiver = _player.GetComponent<PlayerDamageReceiver>();

        _jumpModel = _player.JumpModel;
        _fallingModel = _player.FallingModel;
        _downPlatformModel = _player.DownPlatformModel;
        _poopModel = _player.PoopModel;

        _status = _player.Status;
        _animator = _player.PlayerAnimator;

        _initialGravity = _rigidbody2D.gravityScale;
    }

    public abstract void OnExitState();
    public abstract void EnterState();
    public abstract void Update();

    protected bool IsPlayerTouchingGround => _player.IsTouchingGround;

    /// <summary>
    ///     used on states:
    ///     - Jump
    ///     - Falling
    /// </summary>
    protected void MovePlayerInTheAir()
    {
        FlipPlayerToTheCorrectDirection();

        if (_jumpModel.IsBeingControlledByKnockback && _player.MoveInput.Value.x == 0) return;
        _rigidbody2D.velocity = new Vector2(_status.MovementSpeed.Get() * _player.MoveInput.Value.x, _rigidbody2D.velocity.y);
    }

    protected void FlipPlayerToTheCorrectDirection()
    {
        _player.RotationalTransform.localScale = new Vector3(Mathf.Sign(_player.MoveInput.Value.x), 1, 1);
    }

    /// <param name="playerState"></param>
    /// <returns>true if could really change the State</returns>
    protected bool ChangeState(Player.FiniteState playerState, bool ignoreValidations = false)
    {
        if (!ignoreValidations)
        {
            switch (playerState)
            {
                case Player.FiniteState.Idle:
                    if (!PlayerCanIdle()) return false;
                    break;
                case Player.FiniteState.Move:
                    if (!PlayerCanMove()) return false;
                    break;
                case Player.FiniteState.Jump:
                    if (!PlayerCanJump()) return false;
                    break;
                case Player.FiniteState.Pooping:
                    if (!CanPoop()) return false;
                    break;
            }
        }

        _player.ChangeState(playerState);
        return true;
    }

    protected bool CanGoDownPlatform()
    {
        OneWayPlatform platform = IsOverPlatform();
        if (platform == null) return false;

        return true;
    }

    protected OneWayPlatform IsOverPlatform()
    {
        Collider2D col = _jumpModel.GroundCheck.DrawPhysics2D(_jumpModel.GroundLayer);
        if (col == null) return null;
        return col.GetComponent<OneWayPlatform>();
    }

    protected void ManagePoopPerformedEvent(bool assing)
    {
        if (assing)
        {
            _player.PoopInput.Performed.AddListener(OnPoopPerformed);
        }
        else
        {
            _player.PoopInput.Performed.RemoveListener(OnPoopPerformed);
        }
    }

    protected IEnumerator DeactivateColliderFor()
    {
        _downPlatformModel.IsComingDown = true;
        _downPlatformModel.PlayerCollider.enabled = false;
        yield return new WaitForSeconds(_downPlatformModel.CdwToDeactivateCollider);
        _downPlatformModel.PlayerCollider.enabled = true;
        _downPlatformModel.IsComingDown = false;
    }

    #region VALIDATIONS
    private bool PlayerCanJump()
    {
        return IsPlayerTouchingGround;
    }

    private bool PlayerCanMove() => true;

    private bool PlayerCanIdle()
    {
        return !_downPlatformModel.IsComingDown;
    }

    private bool CanPoop()
    {
        return _poopModel.CanPoop;
    }
    #endregion

    private void OnPoopPerformed()
    {
        ChangeState(Player.FiniteState.Pooping);
    }

    //protected virtual void OnMoveInputStarted() { }

    //protected virtual void OnMoveInputPerformed()
    //{
    //    //FlipPlayer();
    //}

    //protected virtual void OnMoveInputCanceled()
    //{
    //    _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
    //}

    //protected virtual void OnJumpInputStarted() { }

    //protected virtual void OnJumpInputPerformed() { }

    //protected virtual void OnJumpInputCanceled() { }

    //protected virtual void OnPoopInputStarted() 
    //{
    //    //_player.ChangeState(Player.FiniteState.Pooping);
    //}

    //protected void MovePlayerHorizontally()
    //{
    //    if (!_player.CanMove) return;

    //    if (_player.MoveInput.Value.x != 0)
    //    {
    //        _rigidbody2D.drag = 0;
    //        _rigidbody2D.velocity = new Vector2(_player.MoveInput.Value.x * _moveModel.MovementSpeed, _rigidbody2D.velocity.y);
    //    }
    //    else
    //    {
    //        if (_rigidbody2D.velocity.x != 0 && _player.IsOnTheGround() && _rigidbody2D.velocity.y == 0)
    //        {
    //            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);

    //            _rigidbody2D.drag = 10;
    //        }
    //        else
    //        {
    //            _rigidbody2D.drag = 0;
    //        }
    //    }
    //}

    //private void FlipPlayer()
    //{
    //    _player.RotationalTransform.localScale = new Vector3(_player.MoveInput.Value.x > 0 ? 1 : -1, 1, 1);
    //}

}
