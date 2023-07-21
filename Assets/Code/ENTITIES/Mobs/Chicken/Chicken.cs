using Calcatz.MeshPathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Chicken : Enemy
{
    #region INSPECTOR ATTRIBUTES
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
    [field: SerializeField]
    public PlayerPathfinding PlayerPathfinding { get; private set; }
    [field: SerializeField]
    public WormPathfinding WormPathfinding { get; private set; }

    [Header("MY CONFIGURATIONS")]
    [SerializeField]
    private int _maxTier;
    [field: Space]
    [field: SerializeField]
    public ChickenAnimatorModel ChickenAnimator { get; private set; }
    [field: Space]
    [field: SerializeField]
    public ChickenFollowingModel FollowingModel { get; private set; }
    #endregion

    public int CurrentTier { get; private set; }
    public Behaviour? CurrentBehaviour => ((ChickenBaseBehaviour)_currentFiniteBehaviour)?.Behaviour;

    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();
    private BoxCollider2D _boxCollider;

    //Finite Behaviours
    private readonly List<ChickenBaseBehaviour> _finiteBaseBehaviours = new List<ChickenBaseBehaviour>()
    {
        new Patrol(),
        new FollowingPlayer(),
        new FollowingWorm(),
        new AtkWorm(),
        new AtkPlayer(),
        new Die()
    };

    private void OnDrawGizmos()
    {
        PatrolModel.OnDrawGizmos();
        FollowingModel.GroundCheck.DrawGizmos(Color.blue);
        FollowingModel.WallCheck.DrawGizmos(Color.red, false);
    }

    protected override void AfterAwake()
    {
        _behaviourDebug = Behaviour.Born;
        InitializePathfinding();
    }

    #region CALLED BY ANIMATOR EVENTS
    public void ANIMATOR_SetInitialBehaviour()
    {
        CanMove = true;
        this.ChangeBehaviour(Behaviour.Patrol);
    }

    public void ANIMATOR_InteractWithTarget()
    {
        AtkWormModel.InteractWithWorm();
        AtkPlayerModel.InteractWithPlayer();
    }
    public void ANIMATOR_EndMeleeAtkAnimation()
    {
        AtkWormModel.EndAtkAnimation();
        AtkPlayerModel.EndAtkAnimation();
    }

    public void ANIMATOR_ThrowingEgg()
    {
        AtkWormModel.ThrowingEgg();
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
        _currentFiniteBehaviour?.OnExitBehaviour();

        _behaviourDebug = behaviour;
        _currentFiniteBehaviour = _finiteBaseBehaviours.FirstOrDefault(e => e.Behaviour == behaviour);

        _currentFiniteBehaviour.OnEnterBehaviour();
    }

    private void InitializePathfinding()
    {
        PlayerPathfinding.waypoints = GameManager.Waypoints;
        PlayerPathfinding.SetTarget(GameManager.Player.transform);

        WormPathfinding.waypoints = GameManager.Waypoints;
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
