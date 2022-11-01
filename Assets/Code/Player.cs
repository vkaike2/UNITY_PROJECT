using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// TODO: Create some progress bar to show player far
// TODO: One way platform controlls

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    [Header("debug")]
    [SerializeField]
    private State _stateDebug;

    [Header("components")]
    [SerializeField]
    private Transform _mouseIndication;

    [Space]
    [SerializeField]
    private MoveStateModel _moveModel;
    [Space]
    [SerializeField]
    private JumpStateModel _jumpModel;
    [Space]
    [SerializeField]
    private PlayerAnimatorModel _playerAnimator;
    [Space]
    [SerializeField]
    private FartAttributeModel _fartAttribute;

    public MoveStateModel MoveStateModel => _moveModel;
    public JumpStateModel JumpStateModel => _jumpModel;
    public FartAttributeModel FartAttributeModel => _fartAttribute;
    public PlayerAnimatorModel Animator => _playerAnimator;

    public State? PreviousState { get; private set; }
    public State CurrentState => _currentState.State;

    private PlayerBaseState _currentState;
    private List<PlayerBaseState> _states;

    //Finite State
    private readonly PlayerIdleState _playerStateIdle = new PlayerIdleState();
    private readonly PlayerMoveState _playerStateRunning = new PlayerMoveState();
    private readonly PlayerJumpState _playerJumpState = new PlayerJumpState();
    private readonly PlayerFallingState _playerFallingState = new PlayerFallingState();

    //Infinite State
    private readonly PlayerFartState _playerFartState = new PlayerFartState();

    public InputModel<Vector2> MoveInput { get; private set; }
    public InputModel<bool> JumpInput { get; private set; }
    public InputModel<bool> FartInput { get; private set; }

    public bool CanMove { get; set; }

    private void OnDrawGizmos()
    {
        _jumpModel.GroundCheck.DrawGizmos(Color.red);
        _jumpModel.BufferCheck.DrawGizmos(Color.blue);
    }

    private void Awake()
    {
        MoveInput = new InputModel<Vector2>();
        JumpInput = new InputModel<bool>();
        FartInput = new InputModel<bool>();
        FartInput.ClearActions();

        CanMove = true;

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

        _playerFartState.Start(this);
        ChangeState(_states.Where(e => e.ImFistState()).Select(e => e.State).FirstOrDefault());
    }

    private void FixedUpdate()
    {
        this.ManageMouseIndication();
        _currentState.Update();
        _playerFartState.Update();
    }

    public void OnLeftMouseButton(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                FartInput.Started();
                break;
            case InputActionPhase.Performed:
                FartInput.Value = true;
                FartInput.Performed();
                break;
            case InputActionPhase.Canceled:
                FartInput.Value = false;
                FartInput.Canceled();
                break;
        }
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


    private void ManageMouseIndication()
    {
        Vector3 mousePosition = Input.mousePosition;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePosition.x -= objectPos.x;
        mousePosition.y -= objectPos.y;

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        Quaternion mouseRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _mouseIndication.rotation = mouseRotation;

        _mouseIndication.localScale = new Vector3(transform.localScale.x * -1f, 1, 1);
    }

    public enum State
    {
        Idle,
        Move,
        Jump,
        Falling,
        Fart
    }

    public enum Animations
    {
        Idle,
        Move,
        Jump,
        Falling
    }
}
