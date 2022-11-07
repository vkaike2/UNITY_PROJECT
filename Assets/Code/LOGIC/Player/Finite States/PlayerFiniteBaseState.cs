using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerFiniteBaseState
{
    protected Player _player;
    protected Rigidbody2D _rigidbody2D;
    protected JumpStateModel _jumpModel;
    protected MoveStateModel _moveModel;

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
    public abstract void EnterState();
    public abstract void Update();

    protected virtual void OnMoveInputStarted()
    {
        //Debug.Log($"{State} started");
    }
    protected virtual void OnMoveInputPerformed()
    {
        //Debug.Log($"{State} performded");
        FlipPlayer();
    }
    protected virtual void OnMoveInputCanceled()
    {
        _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
        //Debug.Log($"{State}canceled");
    }
    protected virtual void OnJumpInputStarted()
    {
        //Debug.Log($"{State} started");
    }
    protected virtual void OnJumpInputPerformed()
    {
        //Debug.Log($"{State} performded");
    }
    protected virtual void OnJumpInputCanceled()
    {
        //Debug.Log($"{State}canceled");
    }
    protected virtual void OnPoopInputStarted()
    {
        _player.ChangeState(Player.FiniteState.Pooping);
    }

    protected void MovePlayerHorizontally()
    {
        //Debug.Log(_player.CanMove);
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
                _rigidbody2D.drag = 10;
            }
            else
            {
                _rigidbody2D.drag = 0;
            }
        }
    }

    private void FlipPlayer()
    {
        _player.transform.localScale = new Vector3(_player.MoveInput.Value.x > 0 ? 1 : -1, 1, 1);
    }

    protected void DownPlatform()
    {
        OneWayPlatform platform = _player.IsOverPlatform();
        if (platform == null) return;

        platform.PlayerDownPlatform();
        _player.ChangeState(Player.FiniteState.Falling);
    }
}
