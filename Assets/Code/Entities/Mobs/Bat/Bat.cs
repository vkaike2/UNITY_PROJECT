using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public partial class Bat : Enemy
{
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public BatAnimatorModel Animator { get; private set; }

    [field: Header("MODELS")]
    [field: SerializeField]
    public BatPatrolModel PatrolModel { get; private set; }

    protected UnityEvent OnFlapWings { get; private set; } = new UnityEvent();

    public Behaviour? CurrentBehaviour => ((BatBaseBehaviour)_currentFiniteBehaviour)?.Behaviour;
    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();

    private List<BatBaseBehaviour> _finiteBaseBehaviours = new List<BatBaseBehaviour>()
    {
        new Patrol(),
        new FollowingPlayer(),
        new Die()
    };

    private void OnDrawGizmos()
    {
        PatrolModel.DrawGizmos(this.transform.position);
    }

    #region ANIMATOR EVENTS
    public void ANIMATOR_SetInitialBehaviour()
    {
        ChangeBehaviour(Behaviour.Patrol);
    }

    public void ANIMATOR_OnFlapWings()
    {
        OnFlapWings.Invoke();
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
        Patrol,
        FollowingPlayer,
        Die
    }



}
