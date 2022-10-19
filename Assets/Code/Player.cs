using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/* 
 * TODO: 
 * Bug while moving out of some platform  
 */

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    [Header("debug")]
    [SerializeField]
    private State _stateDebug;

    [Space]
    [SerializeField]
    private MoveStateModel _moveModel;
    [Space]
    [SerializeField]
    private JumpStateModel _jumpModel;
    [Space]
    [SerializeField]
    private PlayerAnimatorModel _playerAnimator;

    public MoveStateModel MoveStateModel => _moveModel;
    public JumpStateModel JumpStateModel => _jumpModel;
    public PlayerAnimatorModel Animator => _playerAnimator;

    public State? PreviousState { get; private set; }
    public State CurrentState => _currentState.State;

    private PlayerBaseState _currentState;
    private List<PlayerBaseState> _states;

    private readonly PlayerIdleState _playerStateIdle = new PlayerIdleState();
    private readonly PlayerMoveState _playerStateRunning = new PlayerMoveState();
    private readonly PlayerJumpState _playerJumpState = new PlayerJumpState();
    private readonly PlayerFallingState _playerFallingState = new PlayerFallingState();

    public InputModel<Vector2> MoveInput { get; private set; }
    public InputModel<bool> JumpInput { get; private set; }

    private void OnDrawGizmos()
    {
        _jumpModel.GroundCheck.DrawGizmos(Color.red);
        _jumpModel.BufferCheck.DrawGizmos(Color.blue);
    }

    private void Awake()
    {
        MoveInput = new InputModel<Vector2>();
        JumpInput = new InputModel<bool>();

        _states = new List<PlayerBaseState>()
        {
            _playerStateIdle,
            _playerStateRunning,
            _playerJumpState,
            _playerFallingState
        };
    }

    private void Start()
    {
        foreach (var state in _states)
        {
            state.Start(this);
        }

        ChangeState(_states.Where(e => e.ImFistState()).Select(e => e.State).FirstOrDefault());
    }

    private void FixedUpdate()
    {
        _currentState.Update();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        MoveInput.Value = context.ReadValue<Vector2>();

        switch (context.phase)
        {
            case InputActionPhase.Started:
                MoveInput.Started();
                break;
            case InputActionPhase.Performed:
                MoveInput.Performed();
                break;
            case InputActionPhase.Canceled:
                MoveInput.Canceled();
                break;
        }

    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                JumpInput.Started();
                break;
            case InputActionPhase.Performed:
                JumpInput.Value = true;
                JumpInput.Performed();
                break;
            case InputActionPhase.Canceled:
                JumpInput.Value = false;
                JumpInput.Canceled();
                break;
        }
    }

    public void ChangeState(State state)
    {
        if (_currentState != null)
        {
            PreviousState = _currentState.State;
        }

        _currentState = _states.First(e => e.State == state);
        MoveInput.ClearActions();
        JumpInput.ClearActions();
        _currentState.EnterState();
        _stateDebug = _currentState.State;
    }

    public bool IsOnTheGround()
    {
        Collider2D col = _jumpModel.GroundCheck.DrawPhysics2D(_jumpModel.GroundLayer);
        return col != null;
    }

    public enum State
    {
        Idle,
        Move,
        Jump,
        Falling
    }

    public enum Animations
    {
        Idle,
        Move,
        Jump,
        Falling
    }

    public class OnInput : UnityEvent<InputAction.CallbackContext> { }
}
