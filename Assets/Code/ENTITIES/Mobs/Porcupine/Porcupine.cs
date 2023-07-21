using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Porcupine : Enemy
{
    [Header("DEBUG")]
    [SerializeField]
    private Behaviour _behaviourDebug;

    [field: Header("MY COMPONENTS")]
    [field: SerializeField]
    public PorcupineAnimatorEvents AnimatorEvents { get; private set; }

    [field: Header("MY CONFIGURATIONS")]
    [field: SerializeField]
    public PorcupineAnimatorModel PorculineAnimator { get; private set; }
    [field: Space]
    [field: SerializeField]
    public PorcupinePatrolModel PatrolModel { get; private set; }
    [field: SerializeField]
    public PorcupineAtkModel AtkModel { get; private set; }

    public Behaviour? CurrentBehaviour => ((PorcupineBaseBehaviour)_currentFiniteBehaviour)?.Behaviour;

    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();

    //Finite Behaviours
    private readonly List<PorcupineBaseBehaviour> _finiteBaseBehaviours = new List<PorcupineBaseBehaviour>()
    {
        new Patrol(),
        new Atk(),
        new Die()
    };

    private void OnDrawGizmos()
    {
        PatrolModel.OnDrawGizmos();
    }

    protected override void AfterAwake()
    {
        _behaviourDebug = Behaviour.Born;
    }

    #region ANIMATOR EVENTS
    public void SetInitialBehaviour()
    {
        CanMove = true;
        this.ChangeBehaviour(Behaviour.Patrol);
    }
    #endregion

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
        Atk,
        Die
    }
}
