using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : Entity
{

    [Header("DEBUG")]
    [SerializeField]
    private FiniteState _stateDebug;

    [Space(5)]
    [Header("COMPONENTS")]
    [SerializeField]
    private Hitbox _hitbox;

    [SerializeField] private Transform _rotationalTransform;

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
    private PlayerPoopStateModel _poopModel;

    /* 
        Is called when you poop
        - GameObject: Poop GameObject
    */
    public Action<GameObject> OnPoopEvent { get; set; }

    public Transform RotationalTransform => _rotationalTransform;
    public AudioController AudioController => _audioController;
    public Hitbox Hitbox => _hitbox;
    public PlayerMoveStateModel MoveStateModel => _moveModel;
    public PlayerJumpStateModel JumpStateModel => _jumpModel;
    public PlayerPoopStateModel PoopStateModel => _poopModel;
    public PlayerAnimatorModel Animator => _playerAnimator;
    public FiniteState? PreviousState { get; private set; }
    public FiniteState CurrentState => _currentState.State;
    public UIEventManager UiEventManager => _uiEventManager;
    public PlayerInventory PlayerInventory { get; private set; }
    public InputModel<Vector2> MoveInput { get; private set; }
    public InputModel<bool> JumpInput { get; private set; }
    public InputModel<bool> FartInput { get; private set; }
    public InputModel<bool> DownPlatformInput { get; private set; }
    public InputModel<bool> PoopInput { get; private set; }

    private BoxCollider2D _boxCollider;
    private PlayerFiniteBaseState _currentState;
    private Fart _fart;
    private bool _isFrozen;
    private Rigidbody2D _rigidbody2D;
    //Game Manager
    private GameManager _gameManager;
    private UIEventManager _uiEventManager;

    private readonly List<PlayerFiniteBaseState> _finiteStates = new()
    {
        new PlayerIdleState(),
        new PlayerMoveState(),
        new PlayerJumpState(),
        new PlayerFallingState(),
        new PlayerPoopingState()
    };


    public override bool CanMove { get; set; }

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

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        PlayerInventory = GetComponent<PlayerInventory>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _uiEventManager = GameObject.FindObjectOfType<UIEventManager>();
        _gameManager.SetPlayer(this);

        _fart = GetComponent<Fart>();
    }

    private void Start()
    {
        foreach (var state in _finiteStates)
        {
            state.Start(this);
        }

        ChangeState(GetPossibleState());

        _fart.OnFartEvent.AddListener((fartCdw) =>
        {
            _playerAnimator.PlayAnimationHightPriority(this, PlayerAnimatorModel.Animation.Fart, fartCdw);
        });
    }

    private void FixedUpdate()
    {
        _currentState.Update();
    }

    private void OnDestroy()
    {
        _gameManager.RemovePlayer();
    }

    #region INPUTS

    public void OnDownInput(InputAction.CallbackContext context)
    {
        if (_isFrozen) return;

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
        if (_isFrozen) return;

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
        if (_isFrozen) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                FartInput.Started();
                break;
            case InputActionPhase.Performed:
                _fart.DoFart();
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
        if (_isFrozen) return;

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

    #endregion

    public void ChangeState(FiniteState state)
    {
        if (state == FiniteState.Pooping && !CheckIfCanPoop())
        {
            return;
        }

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

    public void FreezePlayer(bool freeze)
    {
        _isFrozen = freeze;

        if (freeze)
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private IEnumerator DeactivateColliderFor(float seconds)
    {
        _boxCollider.enabled = false;
        yield return new WaitForSeconds(seconds);
        _boxCollider.enabled = true;
    }

    private FiniteState GetPossibleState()
    {
        return _finiteStates.Where(e => e.ImFistState()).Select(e => e.State).FirstOrDefault();
    }

    private bool CheckIfCanPoop()
    {
        if (!_poopModel.CanPoop) return false;

        return true;
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

    public enum FiniteState
    {
        Idle,
        Move,
        Jump,
        Falling,
        Pooping
    }
}