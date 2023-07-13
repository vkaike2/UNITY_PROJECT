using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Calcatz.MeshPathfinding;

public class Worm : Enemy
{
    [Header("DEBUG")]
    [SerializeField]
    private Behaviour _behaviourDebug;

    [Header("MY CONFIGURATIONS")]
    [SerializeField]
    private WormAnimatorModel _wormAnimator;
    [Space]
    [SerializeField]
    private WormPatrolBehaviourModel _patrolModel;
    [Space]
    [SerializeField]
    private WormRebornBehaviourModel _rebornModel;

    public WormAnimatorModel Animator => _wormAnimator;
    public WormPatrolBehaviourModel PatrolModel => _patrolModel;
    public WormRebornBehaviourModel RebornModel => _rebornModel;
    public Behaviour? CurrentBehaviour => ((WormFiniteBaseBehaviour)_currentFiniteBehaviour)?.Behaviour;
    public Pathfinding Pathfinding => _pathfinding;
    public bool IsBeingTargeted { get; set; }

    protected override List<EnemyFiniteBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyFiniteBaseBehaviour)e).ToList();

    private Pathfinding _pathfinding;

    //Finite Behaviours
    private readonly List<WormFiniteBaseBehaviour> _finiteBaseBehaviours = new List<WormFiniteBaseBehaviour>()
    {
        new WormFollowingPlayerBehaviour(),
        new WormPatrolBehaviour(),
        new WormDieBehaviour(),
        new WormRebornBehaviour()
    };

    private void OnDrawGizmos()
    {
        _patrolModel.OnDrawGizmos();
    }

    protected override void AfterAwake()
    {
        _pathfinding = GetComponent<Pathfinding>();
        _behaviourDebug = Behaviour.Born;
    }

    protected override void BeforeStart()
    {
        InitializePathfinding();
        GameManager.SetWorm(this);
    }

    private void OnDestroy()
    {
        GameManager.RemoveWorm(this);
    }

    #region CALLED BY OTHER GAME OBJECTS
    public void InteractWithChicken()
    {
        this.ChangeBehaviour(Behaviour.Reborn);
    }
    #endregion

    #region CALLED BY ANIMATOR EVENTS
    public void SetInitialBehaviour()
    {
        CanMove = true;
        this.ChangeBehaviour(Behaviour.Patrol);
    }
    #endregion

    private void InitializePathfinding()
    {
        _pathfinding.waypoints = GameManager.Waypoints;
        _pathfinding.SetTarget(GameManager.Player.transform);
    }

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

    public enum Behaviour
    {
        Born,
        Patrol,
        FollowingPlayer,
        Die,
        Reborn
    }

}
