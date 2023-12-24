using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Scorpion : Enemy
{
    [field: Header("UI")]
    [field: SerializeField]
    public CdwIndicationUI AttackCdwIndication { get; private set; }

    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public ScorpionAnimatorModel Animator { get; private set; }

    [field: Header("MODELS")]
    [field: SerializeField]
    public ScorpionIdleModel IdleModel { get; private set; }
    [field: SerializeField]
    public ScorpionWalkModel WalkModel { get; private set; }
    [field: SerializeField]
    public ScorpionAttackModel AttackModel { get; private set; }

    public Behaviour? CurrentBehaviour => ((ScorpionBaseBehaviour)_currentFiniteBehaviour)?.Behaviour;
    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();
    private List<ScorpionBaseBehaviour> _finiteBaseBehaviours = new List<ScorpionBaseBehaviour>()
    {
        new Idle(),
        new Walk(),
        new Die()
    };

    private void OnDrawGizmos()
    {
        WalkModel.WillHitTheGround.DrawGizmos();
        WalkModel.WillHitTheWallCheck.DrawGizmos();
    }

    #region ANIMATOR EVENTS
    public void ANIMATOR_SetInitialBehaviour()
    {
        ChangeBehaviour(Behaviour.Idle);
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
        Idle,
        Walk,
        Die
    }
}