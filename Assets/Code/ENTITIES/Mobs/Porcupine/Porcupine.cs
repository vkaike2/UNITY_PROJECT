using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Porcupine : Enemy
{
    [Header("DEBUG")]
    [SerializeField]
    private Behaviour _behaviourDebug;

    [Header("MY COMPONENTS")]
    [SerializeField]
    private PorcupineAnimatorEvents _animatorEvents;

    [Header("MY CONFIGURATIONS")]
    [SerializeField]
    private PorcupineAnimatorModel _porcupineAnimator;
    [Space]
    [SerializeField]
    private PorcupinePatrolBehaviourModel _patrolModel;
    [SerializeField]
    private PorcupineAtkBehaviourModel _atkModel;
    [SerializeField]
    private PorcupineDamageableModel _damageableModel;

    public PorcupineAnimatorEvents AnimatorEvents => _animatorEvents;
    public PorcupineAnimatorModel Animator => _porcupineAnimator;
    public Behaviour? CurrentBehaviour => ((PorcupineFiniteBaseBehaviour)_currentFiniteBehaviour)?.Behaviour;
    public PorcupinePatrolBehaviourModel PatrolModel => _patrolModel;
    public PorcupineAtkBehaviourModel AtkModel => _atkModel;
    public PorcupineDamageableModel DamageableModel => _damageableModel;

    //Finite Behaviours
    private readonly List<PorcupineFiniteBaseBehaviour> _finiteBaseBehaviours = new List<PorcupineFiniteBaseBehaviour>()
    {
        new PorcupinePatrolBehaviour(),
        new PorcupineAtkBehaviour(),
        new PorcupineDieBehaviour()
    };

    //Infinite Behaviours
    private readonly List<EnemyInfiniteBaseBehaviours> _infiniteBaseBehaviours = new List<EnemyInfiniteBaseBehaviours>()
    {
        new PorcupineDamageableBehaviour()
    };

    private void OnDrawGizmos()
    {
        _patrolModel.OnDrawGizmos();
    }

    private void Awake()
    {
        base.Awake();
        _behaviourDebug = Behaviour.Born;

        base.SetFiniteBaseBehaviours(_finiteBaseBehaviours.Select(e => (EnemyFiniteBaseBehaviour)e).ToList());
        base.SetInfiniteBaseBehaviours(_infiniteBaseBehaviours);
    }

    // called by animator events
    public void SetInitialBehaviour()
    {
        CanMove = true;
        this.ChangeBehaviour(Behaviour.Patrol);
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
        Atk,
        Die
    }
}
