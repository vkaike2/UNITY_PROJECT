using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Bird : Enemy
{
    [Header("DEBUG")]
    [SerializeField]
    private Behaviour _behaviourDebug;

    [field: Header("MY CONFIGURATIONS")]
    [field: SerializeField]
    public BridAnimatorModel BirdAnimator { get; private set; }
    [field: Space]
    [field: SerializeField]
    public BirdPatrolModel PatrolModel { get; private set; }
    [field: SerializeField]
    public BirdAtkModel AtkModel { get; private set; }

    public Behaviour? CurrentBehaviour => ((BirdBaseBehaviour)_currentFiniteBehaviour)?.Behaviour;

    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();

    private List<BirdBaseBehaviour> _finiteBaseBehaviours = new List<BirdBaseBehaviour>()
    {
        new Patrol(),
        new Atk(),
        new Die()
    };

    private Rigidbody2D _rigidBody2D;

    protected override void AfterAwake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _rigidBody2D.bodyType = RigidbodyType2D.Static;
    }

    #region ANIMATOR EVENTS
    public void ANIMATOR_SetInitialBehaviour()
    {
        CanMove = true;
        ChangeBehaviour(Behaviour.Patrol);
    }

    public void ANIMATOR_FlappingWings()
    {
        PatrolModel.OnFlappingWings.Invoke();
    }
    #endregion

    public override void Kill()
    {
        ChangeBehaviour(Behaviour.Die);
    }

    public void ChangeBehaviour(Behaviour behaviour)
    {
        _currentFiniteBehaviour?.OnExitBehaviour();

        _behaviourDebug = behaviour;
        _currentFiniteBehaviour = _finiteBaseBehaviours.FirstOrDefault(e => e.Behaviour == behaviour);

        _currentFiniteBehaviour.OnEnterBehaviour();
    }

    public enum Behaviour
    {
        Born,
        Patrol,
        Atk,
        Die
    }
}
