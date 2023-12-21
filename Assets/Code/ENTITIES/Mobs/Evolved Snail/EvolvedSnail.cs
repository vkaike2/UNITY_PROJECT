
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public partial class EvolvedSnail : Enemy
{
    [Header("DEBUG")]
    [SerializeField]
    private Player _debugPlayer;

    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public EvolvedSnailAnimatorModel Animator { get; private set; }

    [field: Header("MODELS")]
    [field: SerializeField]
    public EvolvedSnailIdleModel IdleModel { get; private set; }
    [field: SerializeField]
    public EvolvedSnailFollowingModel FollowingModel { get; private set; }
    [field: SerializeField]
    public EvolvedSnailAttackModel AttackModel { get; set; }

    [field: Header("PATHFINDING")]
    [field: SerializeField]
    public PlayerPathfinding PlayerPathfinding { get; private set; }

    public Behaviour? CurrentBehaviour => ((EvolvedSnailBaseBhaviour)_currentFiniteBehaviour)?.Behaviour;
    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();
    private readonly List<EvolvedSnailBaseBhaviour> _finiteBaseBehaviours = new List<EvolvedSnailBaseBhaviour>()
    {
        new Idle(),
        new FollowingPLayer(),
        new Attack(),
        new Die()
    };

    private void OnDrawGizmos()
    {
        IdleModel.DrawGizmos(this.transform.position);
        FollowingModel.GroundCheck.DrawGizmos();
    }

    protected override void AfterStart()
    {
        InitializePathfinding();
    }

    public override void Kill()
    {
        ChangeBehaviour(Behaviour.Die);
    }

    #region CALLED BY ANIMATOR EVENTS
    public void ANIMATOR_SetInitialBehaviour()
    {
        this.ChangeBehaviour(Behaviour.Idle);
    }

    public void ANIMATOR_OnAttackFrame()
    {
        AttackModel.OnAttackFrame.Invoke();
    }
    #endregion

    public void ChangeBehaviour(Behaviour behaviour)
    {
        _currentFiniteBehaviour?.OnExitBehaviour();
        _currentFiniteBehaviour = _finiteBaseBehaviours.FirstOrDefault(e => e.Behaviour == behaviour);
        _currentFiniteBehaviour.OnEnterBehaviour();
    }

    private void InitializePathfinding()
    {
        StartCoroutine(StartPlayerPathfinding());
    }

    private IEnumerator StartPlayerPathfinding()
    {
        yield return new WaitUntil(() => GameManager.Player != null);
        PlayerPathfinding.waypoints = GameManager.Waypoints;
        PlayerPathfinding.SetTarget(_debugPlayer == null ? GameManager.Player.transform : _debugPlayer.transform);
    }

    public enum Behaviour
    {
        Born,
        Idle,
        FollowingPlayer,
        Atk_Player,
        Die
    }
}