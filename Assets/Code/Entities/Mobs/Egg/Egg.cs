using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Egg : Enemy
{
    [Header("MY CONFIGURATIONS")]
    [SerializeField]
    private EggAnimatorModel _eggAnimator;
    [Space]
    [SerializeField]
    private EggIdleBehaviourModel _idleModel;
    [Space]
    [SerializeField]
    private EggDamageableBehaviourModel _damageableModel;
    [Space]
    [SerializeField]
    private EggSpawningBehaviourModel _spawnableModel;

    public EggAnimatorModel Animator => _eggAnimator;
    public Behaviour? CurrentBehaviour => ((EggFiniteBaseBehaviour)_currentFiniteBehaviour)?.Behaviour;
    public EggIdleBehaviourModel IdleModel => _idleModel;
    public EggDamageableBehaviourModel DamageableModel => _damageableModel;
    public EggSpawningBehaviourModel SpawnableModel => _spawnableModel;

    //Finite Behaviours
    private readonly List<EggFiniteBaseBehaviour> _finiteBaseBehaviours = new List<EggFiniteBaseBehaviour>()
    {
        new EggIdleBehaviour(),
        new EggSpawningBehaviour(),
        new EggDieBehaviour()
    };

    //Infinite Behaviours
    private readonly List<EggInfiniteBaseBehaviour> _infiniteBaseBehaviours = new List<EggInfiniteBaseBehaviour>()
    {
        new EggDamageableBehaviour()
    };

    private void Awake()
    {
        base.Awake();

        base.SetFiniteBaseBehaviours(_finiteBaseBehaviours.Select(e => (EnemyFiniteBaseBehaviour)e).ToList());
        base.SetInfiniteBaseBehaviours(_infiniteBaseBehaviours.Select(e => (EnemyInfiniteBaseBehaviours)e).ToList());
    }

    private void Start()
    {
        base.Start();
        ChangeBehaviour(Behaviour.Idle);
    }

    public void ChangeBehaviour(Behaviour behaviour)
    {
        if (_currentFiniteBehaviour != null)
        {
            _currentFiniteBehaviour.OnExitBehaviour();
        }

        _currentFiniteBehaviour = _finiteBaseBehaviours.FirstOrDefault(e => e.Behaviour == behaviour);

        _currentFiniteBehaviour.OnEnterBehaviour();
    }

    public enum Behaviour
    {
        Idle,
        Spawning,
        Die
    }
}