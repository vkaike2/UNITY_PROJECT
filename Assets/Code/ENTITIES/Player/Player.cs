using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    [Header("DEBUG")]
    [SerializeField]
    private FiniteState _stateDebug;

    [Space(5)]
    [Header("COMPONENTS")]
    [SerializeField]
    private Hitbox _hitbox;
    [SerializeField]
    private Transform _rotationalTransform;

    [Space(5)]
    [Header("ATTRIBUTES")]
    [Tooltip("will deactivate collider for this amount of time")]
    [SerializeField]
    private float _cdwOneWayPlatform = 0.5f;

    [Space(5)]
    [Header("CONFIGURATION")]
    [SerializeField]
    private AudioController _audioController;
    [SerializeField]
    private PlayerMoveStateModel _moveModel;
    [SerializeField]
    private PlayerJumpStateModel _jumpModel;
    [SerializeField]
    private PlayerAnimatorModel _playerAnimator;
    [SerializeField]
    private FartStateModel _fartModel;
    [SerializeField]
    private PlayerPoopStateModel _poopModel;
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
    public UIEventManager UiEventManager => _uiEventManager;

    private BoxCollider2D _boxCollider;
    private PlayerFiniteBaseState _currentState;
    private readonly List<PlayerFiniteBaseState> _finiteStates = new()
    {
        new PlayerIdleState(),
        new PlayerMoveState(),
        new PlayerJumpState(),
        new PlayerFallingState(),
        new PlayerPoopingState()
    };
    private readonly List<PlayerInfiniteBaseState> _infiniteStates = new()
    {
         new PlayerFartState(),
         new PlayerDamageableState()
    };

    //Game Manager
    private GameManager _gameManager;
    private UIEventManager _uiEventManager;
    private CustomMouse _customMouse;

    public InputModel<Vector2> MoveInput { get; private set; }
    public InputModel<bool> JumpInput { get; private set; }
    public InputModel<bool> FartInput { get; private set; }
    public InputModel<bool> DownPlatformInput { get; private set; }
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
        JumpInput = new InputModel<bool>();
        FartInput = new InputModel<bool>();
        DownPlatformInput = new InputModel<bool>();
        PoopInput = new InputModel<bool>();
        ClearAllInputActions();

        CanMove = true;

        _boxCollider = GetComponent<BoxCollider2D>();

        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _customMouse = GameObject.FindObjectOfType<CustomMouse>();
        _uiEventManager = GameObject.FindObjectOfType<UIEventManager>();
        _gameManager.SetPlayer(this);
    }

    private void ClearFiniteInputActions()
    {
        MoveInput.ClearActions();
        JumpInput.ClearActions();
        DownPlatformInput.ClearActions();
        PoopInput.ClearActions();
    }

    private void ClearAllInputActions()
    {
        FartInput.ClearActions();
        ClearFiniteInputActions();
    }

    private void Start()
    {
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
                DownPlatformInput.Started();
                break;
            case InputActionPhase.Performed:
                DownPlatformInput.Value = true;
                DownPlatformInput.Performed();
                break;
            case InputActionPhase.Canceled:
                DownPlatformInput.Value = false;
                DownPlatformInput.Canceled();
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
                JumpInput.Started();
                break;
            case InputActionPhase.Performed:
                JumpInput.Value = true;
                JumpInput.Performed();
                break;
            case InputActionPhase.Canceled:
                DownPlatformInput.Value = false;
                JumpInput.Value = false;
                break;
        }
    }

    public void ChangeState(FiniteState state)
    {
        if (_currentState != null)
        {
            PreviousState = _currentState.State;
            _currentState.OnExitState();
            ClearFiniteInputActions();
        }

        _currentState = _finiteStates.First(e => e.State == state);

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

    public void DownPlatform()
    {
        if (!_boxCollider.enabled) return;
        StartCoroutine(DeactivateColliderFor(_cdwOneWayPlatform));
    }

    public IEnumerator DeactivateColliderFor(float seconds)
    {
        _boxCollider.enabled = false;
        yield return new WaitForSeconds(seconds);
        _boxCollider.enabled = true;
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
