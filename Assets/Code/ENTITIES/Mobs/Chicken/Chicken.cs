using Calcatz.MeshPathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chicken : Enemy
{
    [Header("DEBUG")]
    [SerializeField]
    private Behaviour _behaviourDebug;
    [SerializeField]
    private int _debugCurrentTier;

    [Header("CANVAS")]
    [SerializeField]
    private MobTier _tierComponent;

    [Header("MY COMPONENTS")]
    [SerializeField]
    private ChickenAnimatorEvents _animatorEvents;
    [SerializeField]
    private PlayerPathfinding _playerPathfinding;
    [SerializeField]
    private WormPathfinding _wormPathfinding;

    [Header("MY CONFIGURATIONS")]
    [SerializeField]
    private int _maxTier;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private ChickenAnimatorModel _chickenAnimator;
    [SerializeField]
    private ChickenPatrolBehaviourModel _patrolModel;
    [SerializeField]
    private ChickenFollowingBehaviourModel _followingPlayerModel;
    [SerializeField]
    private ChickenAtkWormBehaviourModel _atkWormModel;
    [SerializeField]
    private ChickenAtkPlayerBehaviourModel _atkPlayerModel;
    [SerializeField]
    private ChickenDamageableBehaviourModel _damageableModel;
    
    public ChickenAnimatorModel Animator => _chickenAnimator;
    public ChickenPatrolBehaviourModel PatrolModel => _patrolModel;
    public ChickenFollowingBehaviourModel FollowingPlayerModel => _followingPlayerModel;
    public ChickenAtkWormBehaviourModel AtkWormModel => _atkWormModel;
    public ChickenAtkPlayerBehaviourModel AtkPlayerModel => _atkPlayerModel;
    public ChickenDamageableBehaviourModel DamageableModel => _damageableModel;
    public PlayerPathfinding PlayerPathfinding => _playerPathfinding;
    public WormPathfinding WormPathfinding => _wormPathfinding;
    public BoxCollider2D BoxCollider2D => _boxCollider;
    public int CurrentTier { get; private set; }
    public Behaviour? CurrentBehaviour => ((ChickenFiniteBaseBehaviour)_currentFiniteBehaviour)?.Behaviour;

    private BoxCollider2D _boxCollider;


    //Finite Behaviours
    private readonly List<ChickenFiniteBaseBehaviour> _finiteBaseBehaviours = new List<ChickenFiniteBaseBehaviour>()
    {
        new ChickenPatrolBehaviour(),
        new ChickenFollowingPlayerBehaviour(),
        new ChickenFollowingWormBehaviour(),
        new ChickenAtkWormBehaviour(),
        new ChickenAtkPlayerBehaviour(),
        new ChickenDieBehaviour()
    };

    //Infinite Behaviours
    private readonly List<ChickenInfiniteBaseBehaviour> _infiniteBaseBehaviours = new List<ChickenInfiniteBaseBehaviour>()
    {
        new ChickenDamageableBehaviour()
    };

    private void OnDrawGizmos()
    {
        _patrolModel.OnDrawGizmos();
        _followingPlayerModel.GroundCheck.DrawGizmos(Color.blue);
        _followingPlayerModel.WallCheck.DrawGizmos(Color.red, false);
        _followingPlayerModel.GizmosTest();
    }

    private void Awake()
    {
        base.Awake();

        _boxCollider = GetComponent<BoxCollider2D>();

        _behaviourDebug = Behaviour.Born;

        base.SetFiniteBaseBehaviours(_finiteBaseBehaviours.Select(e => (EnemyFiniteBaseBehaviour)e).ToList());
        base.SetInfiniteBaseBehaviours(_infiniteBaseBehaviours.Select(e => (EnemyInfiniteBaseBehaviours)e).ToList());
    }

    private void Start()
    {
        InitializePathfinding();
        base.Start();
    }

    #region CALLED BY ANIMATOR EVENTS
    public void SetInitialBehaviour()
    {
        CanMove = true;
        this.ChangeBehaviour(Behaviour.Patrol);
    }

    public void InteractWithTarget()
    {
        _atkWormModel.InteractWithWorm();
        _atkPlayerModel.InteractWithPlayer();
    }
    public void EndMeleeAtkAnimation()
    {
        _atkWormModel.EndAtkAnimation();
        _atkPlayerModel.EndAtkAnimation();
    }

    public void ThrowingEgg()
    {
        _atkWormModel.ThrowingEgg();
    }
    #endregion

    public void AddTier()
    {
        if (CurrentTier >= _maxTier) return;

        _status.MovementSpeed.IncreasePercentage(0.3f);
        CurrentTier += 1;
        _debugCurrentTier = CurrentTier;
        _tierComponent.AddTier();
    }

    public bool IsMaxTier() => CurrentTier == _maxTier;

    public void ChangeBehaviour(Behaviour behaviour)
    {
        if (_currentFiniteBehaviour != null)
        {
            _currentFiniteBehaviour.OnExitBehaviour();
        }

        _behaviourDebug = behaviour;
        _currentFiniteBehaviour = _finiteBaseBehaviours.FirstOrDefault(e => e.Behaviour == behaviour);

        _currentFiniteBehaviour.OnEnterBehaviour();
    }

    private void InitializePathfinding()
    {
        _playerPathfinding.waypoints = GameManager.Waypoints;
        _playerPathfinding.SetTarget(GameManager.Player.transform);

        _wormPathfinding.waypoints = GameManager.Waypoints;
    }

    public enum Behaviour
    {
        Born,
        Patrol,
        FollowingPlayer,
        FollowingWorm,
        Atk_Worm,
        Atk_Player,
        Die
    }
}
