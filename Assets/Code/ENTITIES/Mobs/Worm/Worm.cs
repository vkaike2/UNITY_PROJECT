using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Calcatz.MeshPathfinding;

public partial class Worm : Enemy
{
    [Header("DEBUG")]
    [SerializeField]
    private Behaviour _behaviourDebug;

    [field: Header("MY CONFIGURATIONS")]
    [field: SerializeField]
    public WormAnimatorModel WormAnimator { get; private set; }
    [field: Space]
    [field: SerializeField]
    public WormPatrolModel PatrolModel { get; private set; }
    [field: SerializeField]
    public WormRebornModel RebornModel { get; private set; }

    public Behaviour? CurrentBehaviour => ((WormBaseBehaviour)_currentFiniteBehaviour)?.Behaviour;
    public Pathfinding Pathfinding => _pathfinding;
    public bool IsBeingTargeted { get; set; }

    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();

    private Pathfinding _pathfinding;

    //Finite Behaviours
    private readonly List<WormBaseBehaviour> _finiteBaseBehaviours = new List<WormBaseBehaviour>()
    {
        new FollowingPlayer(),
        new Patrol(),
        new Die(),
        new Reborn()
    };

    private void OnDrawGizmos()
    {
        PatrolModel.OnDrawGizmos();
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
    public override void Kill()
    {
        ChangeBehaviour(Behaviour.Die);
    }

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
