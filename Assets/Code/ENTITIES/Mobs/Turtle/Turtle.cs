using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Turtle : Enemy
{
    [field: Header("UI")]
    [field: SerializeField]
    public CdwIndicationUI CdwIndication { get; private set; }

    [field: Header("ATTACKS")]
    [field: SerializeField]
    public float CdwBetweenShoots { get; set; } = 3f;
    [field: SerializeField]
    public float ProjectileSpeed { get; set; } = 5f;
    [field: SerializeField]
    public float ProjectileDuration { get; set; }

    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public TurtleAnimatorModel Animator { get; private set; }

    [field: Header("MODELS")]
    [field: SerializeField]
    public TurtleWalkModel WalkModel { get; set; }

    public Behaviour? CurrentBehaviour => ((TurtleBaseBehaviour)_currentFiniteBehaviour)?.Behaviour;
    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();

    private List<TurtleBaseBehaviour> _finiteBaseBehaviours = new List<TurtleBaseBehaviour>()
    {
        new Walk(),
        new Die()
    };

    private void OnDrawGizmos()
    {
        WalkModel.WillHitTheWallCheck.DrawGizmos();
        WalkModel.WillHitTheGround.DrawGizmos();
    }

    #region ANIMATOR EVENTS
    public void ANIMATOR_SetInitialBehaviour()
    {
        ChangeBehaviour(Behaviour.Walk);
    }
    #endregion


    public override void Kill()
    {
        ChangeBehaviour(Behaviour.Die);
    }

    public void ChangeBehaviour(Behaviour behaviour)
    {
        _currentFiniteBehaviour?.OnExitBehaviour();
        _currentFiniteBehaviour = _finiteBaseBehaviours.FirstOrDefault(e => e.Behaviour == behaviour);
        _currentFiniteBehaviour.OnEnterBehaviour();
    }

    public enum Behaviour
    {
        Born,
        Walk,
        Die
    }
}