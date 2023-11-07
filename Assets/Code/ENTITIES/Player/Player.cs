using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    [Header("DEBUG")]
    [SerializeField]
    private FiniteState _stateDebug;

    [field: Space]
    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public bool CanMove { get; set; }

    [field: Space]
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public Hitbox Hitbox { get; private set; }

    [field: SerializeField]
    public Transform RotationalTransform { get; private set; }
    [field: SerializeField]
    public AudioController AudioController { get; private set; }
    [field: SerializeField]
    public PlayerAnimatorModel PlayerAnimator { get; private set; }

    [field: Space]
    [field: Header("STATE MODELS")]
    [field: SerializeField]
    public PlayerJumpModel JumpModel { get; private set; }
    [field: SerializeField]
    public PlayerFallingModel FallingModel { get; private set; }
    [field: SerializeField]
    public PlayerDownPlatformModel DownPlatformModel { get; private set; }
    [field: SerializeField]
    public PlayerPoopModel PoopModel { get; private set; }


    public FiniteState CurrentState => _currentState.State;
    public UIEventManager UIEventManager { get; private set; }
    public PlayerInventory PlayerInventory { get; private set; }
    public PlayerStatus Status { get; private set; }
    public PlayerDamageDealer DamageDealer { get; set; }

    private Fart _fart;
    private bool _isFrozen;

    private PlayerBaseState _currentState;
    private Rigidbody2D _rigidbody2D;
    private GameManager _gameManager;
    private MapManager _mapManager;


    private readonly List<PlayerBaseState> _finiteStates = new()
    {
        new Idle(),
        new Move(),
        new Jump(),
        new Falling(),
        new Pooping()
    };

    private void OnDrawGizmos()
    {
        JumpModel.GroundCheck.DrawGizmos();
        JumpModel.BufferCheck.DrawGizmos();
    }

    private void Awake()
    {
        Status = GetComponent<PlayerStatus>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        PlayerInventory = GetComponent<PlayerInventory>();
        DamageDealer = GetComponent<PlayerDamageDealer>();
        _fart = GetComponent<Fart>();
    }

    private void Start()
    {
        InitializeManagers();

        PlayerInventory.LoadInventoryDataFromMemory();

        foreach (var state in _finiteStates)
        {
            state.Start(this);
        }

        ChangeState(GetFirstState());
    }

    private void FixedUpdate()
    {
        _currentState?.Update();
    }

    private void OnDestroy()
    {
        _gameManager.RemovePlayer();
    }

    public void ChangeState(FiniteState state)
    {
        _currentState?.OnExitState();

        _currentState = _finiteStates.First(e => e.State == state);

        _currentState.EnterState();
        _stateDebug = _currentState.State;
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

    private FiniteState GetFirstState()
    {
        return _finiteStates.Where(e => e.ImFistState()).Select(e => e.State).FirstOrDefault();
    }

    private void InitializeManagers()
    {
        _mapManager = GameObject.FindObjectOfType<MapManager>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        UIEventManager = GameObject.FindObjectOfType<UIEventManager>();

        _gameManager.SetPlayer(this);
    }

    public enum FiniteState
    {
        Idle,
        Move,
        Jump,
        Falling,
        Pooping
    }

    #region OLD CODE
    //public bool IsOnTheGround()
    //{
    //    Collider2D col = _jumpModel.GroundCheck.DrawPhysics2D(_jumpModel.GroundLayer);
    //    return col != null;
    //}

    //public OneWayPlatform IsOverPlatform()
    //{
    //    Collider2D col = _jumpModel.GroundCheck.DrawPhysics2D(_jumpModel.GroundLayer);
    //    if (col == null) return null;
    //    return col.GetComponent<OneWayPlatform>();
    //}

    //public void DownPlatform()
    //{
    //    if (!_boxCollider.enabled) return;
    //    StartCoroutine(DeactivateColliderFor(_cdwOneWayPlatform));
    //}

    //private IEnumerator DeactivateColliderFor(float seconds)
    //{
    //    _boxCollider.enabled = false;
    //    yield return new WaitForSeconds(seconds);
    //    _boxCollider.enabled = true;
    //}

    //private bool CheckIfCanPoop()
    //{
    //    if (!_poopModel.CanPoop) return false;

    //    return true;
    //}

    //private void ClearFiniteInputActions()
    //{
    //    MoveInput.ClearActions();
    //    JumpInput.ClearActions();
    //    DownPlatformInput.ClearActions();
    //    PoopInput.ClearActions();
    //}

    //private void ClearAllInputActions()
    //{
    //    FartInput.ClearActions();
    //    ClearFiniteInputActions();
    //}
    #endregion
}