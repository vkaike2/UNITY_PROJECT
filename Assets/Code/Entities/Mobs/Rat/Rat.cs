using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Rat : Enemy
{
    [Header("DEBUG")]
    [SerializeField]
    private Behaviour _behaviourDebug;

    [field: Header("MY COMPONENTS")]
    [field: SerializeField]
    public PlayerPathfinding PlayerPathfinding {  get; private set; }

    [field: Header("MY CONFIGURATIONS")]
    [field: SerializeField]
    public RatAnimatorModel RatAnimator { get; private set; }
    [field:Space]
    [field: SerializeField]
    public RatIdleModel IdleModel { get; private set; }
    [field: SerializeField]
    public RatFollowingModel FollowingModel { get; private set; }

    public Behaviour? CurrentBehaviour => ((RatBaseBehaviour)_currentFiniteBehaviour)?.Behaviour;

    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();


    private List<RatBaseBehaviour> _finiteBaseBehaviours = new List<RatBaseBehaviour>()
    {
        new Idle(),
        new FollowingPlayer(),
        new Die()
    };

    private void OnDrawGizmos()
    {
        FollowingModel.GroundCheck.DrawGizmos();
        FollowingModel.WallCheck.DrawGizmos();
    }

    protected override void AfterAwake()
    {
        InitializePathfinding();
    }

    #region ANIMATOR EVENTS
    public void ANIMATOR_SetInitialBehaviour()
    {
        CanMove = true;
        ChangeBehaviour(Behaviour.Idle);
    }
    #endregion

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
    }

    public enum Behaviour
    {
        Born,
        Idle,
        FollowingPlayer,
        Die
    }
}
