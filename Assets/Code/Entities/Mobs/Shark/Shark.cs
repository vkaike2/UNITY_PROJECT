using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Shark : Enemy
{
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public SharkAnimatorModel Animator { get; private set; }

    [field: Header("MODELS")]
    [field: SerializeField]
    public SharkWalkModel WalkModel { get; set; }
    [field: SerializeField]
    public SharkAttackModel AttackModel { get; set; }

    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();
    public Behaviour CurrentBehaviour { get; set; }

    private List<SharkBaseBehaviour> _finiteBaseBehaviours = new List<SharkBaseBehaviour>()
    {
        new Born(),
        new Walk(),
        new Attack(),
        new Die()
    };

    private void OnDrawGizmos()
    {
        WalkModel.WillHitTheWallCheck.DrawGizmos();
        WalkModel.WillHitTheGround.DrawGizmos();
    }

    public override void Kill()
    {
        ChangeBehaviour(Behaviour.Die);
    }

    #region ANIMATOR EVENTS
    public void ANIMATOR_SetInitialBehaviour()
    {
        ChangeBehaviour(Behaviour.Walk);
    }
    public void ANIMATOR_FinishedAttackAnimation()
    {
        AttackModel.OnAttackFinished.Invoke();
    }
    #endregion

    public void ChangeBehaviour(Behaviour behaviour)
    {
        _currentFiniteBehaviour?.OnExitBehaviour();
        _currentFiniteBehaviour = _finiteBaseBehaviours.FirstOrDefault(e => e.Behaviour == behaviour);
     
        _currentFiniteBehaviour.OnEnterBehaviour();
        CurrentBehaviour = behaviour;
    }

    public enum Behaviour
    {
        Born,
        Walk,
        Attack,
        Die
    }
}