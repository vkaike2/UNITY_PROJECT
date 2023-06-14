using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerFiniteBaseState
{
    protected Player _player;
    protected Rigidbody2D _rigidbody2D;
    protected PlayerJumpStateModel _jumpModel;
    protected PlayerMoveStateModel _moveModel;

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
        _initialGravity = _rigidbody2D.gravityScale;

        _jumpModel = _player.JumpStateModel;
        _moveModel = _player.MoveStateModel;
    }

    public virtual void OnExitState() { }
    public abstract void EnterState();
    public abstract void Update();

    protected virtual void OnMoveInputStarted() { }

    protected virtual void OnMoveInputPerformed()
    {
        FlipPlayer();
    }

    protected virtual void OnMoveInputCanceled()
    {
        _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
    }

    protected virtual void OnJumpInputStarted() { }

    protected virtual void OnJumpInputPerformed() { }

    protected virtual void OnJumpInputCanceled() { }

    protected virtual void OnPoopInputStarted()
    {
        _player.ChangeState(Player.FiniteState.Pooping);
    }

    protected void MovePlayerHorizontally()
    {
        if (!_player.CanMove) return;

        if (_player.MoveInput.Value.x != 0)
        {
            _rigidbody2D.drag = 0;
            _rigidbody2D.velocity = new Vector2(_player.MoveInput.Value.x * _moveModel.MovementSpeed, _rigidbody2D.velocity.y);
        }
        else
        {
            if (_rigidbody2D.velocity.x != 0 && _player.IsOnTheGround() && _rigidbody2D.velocity.y == 0)
            {
                _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);

                _rigidbody2D.drag = 10;
            }
            //else if( _rigidbody2D.velocity.x == 0 && _player.IsOnTheGround())
            //{
            //    _rigidbody2D.velocity = Vector2.zero;
            //}
            else
            {
                _rigidbody2D.drag = 0;
            }
        }
    }

    private void FlipPlayer()
    {
        _player.RotationalTransform.localScale = new Vector3(_player.MoveInput.Value.x > 0 ? 1 : -1, 1, 1);
    }

    protected void DownPlatform()
    {
        OneWayPlatform platform = _player.IsOverPlatform();
        if (platform == null) return;

        _player.DownPlatform();
        _player.ChangeState(Player.FiniteState.Falling);
    }
}
