using System;
using System.Collections;
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
    [field: SerializeField]
    public PlayerEatModel EatModel { get; private set; }
    [field: SerializeField]
    public PlayerDieModel DieModel { get; set; }

    public FiniteState CurrentState => _currentState.State;
    public UIEventManager UIEventManager { get; private set; }
    public PlayerInventory PlayerInventory { get; private set; }
    public PlayerStatus Status { get; private set; }
    public PlayerDamageDealer DamageDealer { get; set; }
    public bool IsTouchingGround => JumpModel.GroundCheck.DrawPhysics2D(JumpModel.GroundLayer) != null;

    protected GameManager GameManager { get; private set; }
    protected PlayerDamageReceiver DamageReceiver { get; private set; }

    protected OnKnockbackEvent OnKnockbackEvent { get; private set; } = new OnKnockbackEvent();
    public KnockBackInformation KnockBackInfo { get; private set; } = new KnockBackInformation();


    private bool _isFrozen;

    private Fart _fart;
    private PlayerBaseState _currentState;
    private Rigidbody2D _rigidBody2D;
    private MapManager _mapManager;

    private readonly List<PlayerBaseState> _finiteStates = new()
    {
        new Idle(),
        new Move(),
        new Jump(),
        new Falling(),
        new Pooping(),
        new Eating(),
        new Dead()
    };

    private void OnDrawGizmos()
    {
        JumpModel.GroundCheck.DrawGizmos();
        JumpModel.BufferCheck.DrawGizmos();
    }

    private void Awake()
    {
        Status = GetComponent<PlayerStatus>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        PlayerInventory = GetComponent<PlayerInventory>();
        DamageDealer = GetComponent<PlayerDamageDealer>();
        DamageReceiver = GetComponent<PlayerDamageReceiver>();
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

        DamageReceiver.OnKnockbackEvent.AddListener(HandleKnockBack);
        _fart.OnKnockBackEvent.AddListener(HandleKnockBack);
    }

    private void FixedUpdate()
    {
        _currentState?.Update();
    }

    private void OnDestroy()
    {
        GameManager.RemovePlayer();
    }

    /// <summary>
    ///    
    /// </summary>
    /// <param name="checkInventory"> Check if there is something to eat on Inventory </param>
    /// <returns></returns>
    public bool CanEat(bool checkInventory)
    {
        if (checkInventory)
        {
            return IsTouchingGround && PlayerInventory.CanEatItem();

        }
        return IsTouchingGround;
    }

    public void Eat(ItemData item)
    {
        EatModel.EatingItemData = item;
        ChangeState(FiniteState.Eating);
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
            _rigidBody2D.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            _rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private FiniteState GetFirstState()
    {
        return _finiteStates.Where(e => e.ImFistState()).Select(e => e.State).FirstOrDefault();
    }

    private void InitializeManagers()
    {
        _mapManager = GameObject.FindObjectOfType<MapManager>();
        GameManager = GameObject.FindObjectOfType<GameManager>();
        UIEventManager = GameObject.FindObjectOfType<UIEventManager>();

        GameManager.SetPlayer(this);
    }


    /// <param name="seconds"> how many seconds player will be controller by the Knockback action</param>
    /// <exception cref="NotImplementedException"></exception>
    private void HandleKnockBack(float seconds, KnockBackSource source)
    {
        OnKnockbackEvent.Invoke(seconds, source);
        StartCoroutine(IsBeingControllerByKnockback(seconds, source));
    }

    private IEnumerator IsBeingControllerByKnockback(float seconds, KnockBackSource source)
    {
        KnockBackInfo.Source = source;
        KnockBackInfo.IsBeingControlledByKnockBack = true;
        yield return new WaitForSeconds(seconds);
        KnockBackInfo.IsBeingControlledByKnockBack = false;
    }

    public class KnockBackInformation
    {
        public bool IsBeingControlledByKnockBack { get; set; }
        public KnockBackSource Source { get; set; }
    }

    public enum FiniteState
    {
        Idle,
        Move,
        Jump,
        Falling,
        Pooping,
        Eating,
        Dead
    }
}