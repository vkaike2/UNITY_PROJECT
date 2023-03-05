using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    [Header("debug")]
    [SerializeField]
    private FiniteState _stateDebug;

    [Space]
    [Header("components")]
    [SerializeField]
    private Hitbox _hitbox;

    [Space]
    [Header("configuration")]
    [SerializeField]
    private Transform _rotationalTransform;
    [Space]
    [SerializeField]
    private AudioController _audioController;
    [Space]
    [SerializeField]
    private PlayerMoveStateModel _moveModel;
    [Space]
    [SerializeField]
    private PlayerJumpStateModel _jumpModel;
    [Space]
    [SerializeField]
    private PlayerAnimatorModel _playerAnimator;
    [Space]
    [SerializeField]
    private FartStateModel _fartModel;
    [Space]
    [SerializeField]
    private PlayerPoopStateModel _poopModel;
    [Space]
    [SerializeField]
    private PlayerDamageableStateModel _damageableModel;

    /// <summary>
    ///     Is called when you poop
    /// - GameObject: Poop GameObject
    /// </summary>
    public Action<GameObject> OnPoopEvent { get; set; }

    public Transform RotationalTransform => _rotationalTransform;
    public AudioController AudioController => _audioController;

    public Hitbox Hitbox => _hitbox;

    public PlayerMoveStateModel MoveStateModel => _moveModel;
    public PlayerJumpStateModel JumpStateModel => _jumpModel;
    public PlayerPoopStateModel PoopStateModel => _poopModel;
    public FartStateModel FartStateModel => _fartModel;
    public PlayerDamageableStateModel DamageableStateModel => _damageableModel;

    public PlayerAnimatorModel Animator => _playerAnimator;

    public FiniteState? PreviousState { get; private set; }
    public FiniteState CurrentState => _currentState.State;

    private PlayerFiniteBaseState _currentState;
    private List<PlayerFiniteBaseState> _finiteStates;
    private List<PlayerInfiniteBaseState> _infiniteStates;

    public UIEventManager UiEventManager => _uiEventManager;

    //Game Manager
    private GameManager _gameManager;
    private UIEventManager _uiEventManager;


    //Finite State
    private readonly PlayerIdleState _playerStateIdle = new PlayerIdleState();
    private readonly PlayerMoveState _playerStateRunning = new PlayerMoveState();
    private readonly PlayerJumpState _playerJumpState = new PlayerJumpState();
    private readonly PlayerFallingState _playerFallingState = new PlayerFallingState();
    private readonly PlayerPoopingState _playerPoopingState = new PlayerPoopingState();

    //Infinite State
    private readonly PlayerFartState _playerFartState = new PlayerFartState();
    private readonly PlayerDamageableState _playerDamageableState = new PlayerDamageableState();

    public InputModel<Vector2> MoveInput { get; private set; }
    public InputModel<bool> JumpInput { get; private set; }
    public InputModel<bool> FartInput { get; private set; }
    public InputModel<bool> DownInput { get; private set; }
    public InputModel<bool> DownPlatform { get; private set; }
    public InputModel<bool> PoopInput { get; private set; }

    public bool CanMove { get; set; }

    private void OnDrawGizmos()
    {
        _jumpModel.GroundCheck.DrawGizmos(Color.red);
        _jumpModel.BufferCheck.DrawGizmos(Color.blue);
    }

    private void Awake()
    {
        MoveInput = new InputModel<Vector2>();
        MoveInput.ClearActions();
        JumpInput = new InputModel<bool>();
        JumpInput.ClearActions();
        FartInput = new InputModel<bool>();
        FartInput.ClearActions();
        DownInput = new InputModel<bool>();
        DownInput.ClearActions();
        DownPlatform = new InputModel<bool>();
        DownPlatform.ClearActions();
        PoopInput = new InputModel<bool>();
        PoopInput.ClearActions();

        CanMove = true;

        _finiteStates = new List<PlayerFiniteBaseState>()
        {
            _playerStateIdle,
            _playerStateRunning,
            _playerJumpState,
            _playerFallingState,
            _playerPoopingState
        };

        _infiniteStates = new List<PlayerInfiniteBaseState>()
        {
            _playerFartState,
            _playerDamageableState
        };

    }

    private void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _uiEventManager = GameObject.FindObjectOfType<UIEventManager>();
        
        _gameManager.SetPlayer(this);

        foreach (var state in _finiteStates)
        {
            state.Start(this);
        }

        foreach (var state in _infiniteStates)
        {
            state.Start(this);
        }

        ChangeState(GetPossibleState());
    }

    private void FixedUpdate()
    {
        _currentState.Update();

        foreach (var state in _infiniteStates)
        {
            state.Update();
        }
    }

    private void OnDestroy()
    {
        _gameManager.RemovePlayer();
    }

    public void OnDownInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                DownInput.Started();
                break;
            case InputActionPhase.Performed:
                DownInput.Value = true;
                DownInput.Performed();
                break;
            case InputActionPhase.Canceled:
                DownInput.Value = false;
                DownInput.Canceled();
                break;
        }
    }

    public void OnRightMouseButton(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                PoopInput.Started();
                break;
            case InputActionPhase.Performed:
                PoopInput.Value = true;
                PoopInput.Performed();
                break;
            case InputActionPhase.Canceled:
                PoopInput.Value = false;
                PoopInput.Canceled();
                break;
        }
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
                if (DownInput.Value) return;

                JumpInput.Started();
                break;
            case InputActionPhase.Performed:
                if (DownInput.Value)
                {
                    DownPlatform.Value = true;
                    DownPlatform.Performed();
                }
                else
                {
                    JumpInput.Value = true;
                    JumpInput.Performed();
                }
                break;
            case InputActionPhase.Canceled:
                DownPlatform.Value = false;
                JumpInput.Value = false;
                JumpInput.Canceled();
                break;
        }
    }

    public void ChangeState(FiniteState state)
    {
        if (_currentState != null)
        {
            PreviousState = _currentState.State;
            _currentState.OnExitState();
        }


        _currentState = _finiteStates.First(e => e.State == state);
        MoveInput.ClearActions();
        JumpInput.ClearActions();
        PoopInput.ClearActions();

        _currentState.EnterState();
        _stateDebug = _currentState.State;
    }

    public bool IsOnTheGround()
    {
        Collider2D col = _jumpModel.GroundCheck.DrawPhysics2D(_jumpModel.GroundLayer);
        return col != null;
    }

    public (Vector2 position, Vector2 direction, Quaternion rotation) GetMouseInformationRelatedToPlayer()
    {
        Vector2 mousePosition = GetMousePosition();

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        Quaternion mouseRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        return (mousePosition, GetMouseDirectionRelatedToPlayer(), mouseRotation);
    }

    public Vector2 GetMouseDirectionRelatedToPlayer()
    {
        return GetMousePosition().normalized;
    }

    public Vector2 GetMousePosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePosition.x -= objectPos.x;
        mousePosition.y -= objectPos.y;
        return mousePosition;
    }

    public OneWayPlatform IsOverPlatform()
    {
        Collider2D col = _jumpModel.GroundCheck.DrawPhysics2D(_jumpModel.GroundLayer);
        if (col == null) return null;
        return col.GetComponent<OneWayPlatform>();
    }

    public FiniteState GetPossibleState()
    {
        return _finiteStates.Where(e => e.ImFistState()).Select(e => e.State).FirstOrDefault();
    }

    public enum FiniteState
    {
        Idle,
        Move,
        Jump,
        Falling,
        Pooping
    }
}
