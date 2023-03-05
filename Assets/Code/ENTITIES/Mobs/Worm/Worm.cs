using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Worm : Enemy
{
    [Header("debug")]
    [SerializeField]
    private Behaviour _behaviourDebug;
    
    [Header("configuration")]
    [SerializeField]
    private WormAnimatorModel _wormAnimator;
    [Space]
    [SerializeField]
    private WormPatrolBehaviourModel _patrolModel;
    [Space]
    [SerializeField]
    private WormDamageableBehavourModel _damageableModel;

    public WormAnimatorModel Animator => _wormAnimator;
    public WormPatrolBehaviourModel PatrolModel => _patrolModel;
    public WormDamageableBehavourModel DamageableModel => _damageableModel;

    public Behaviour? CurrentBehaviour => ((WormFiniteBaseBehaviour) _currentFiniteBehaviour)?.Behaviour;

    //Finite Behaviours
    private readonly List<WormFiniteBaseBehaviour> _finiteBaseBehaviours = new List<WormFiniteBaseBehaviour>()
    {
        new WormFollowingPlayerBehaviour(), 
        new WormPatrolBehaviour(),
        new WormDieBehaviour()
    };

    //Infinite Behaviours
    private readonly List<WormInifiniteBaseBehaviour> _infiniteBaseBehaviours = new List<WormInifiniteBaseBehaviour>()
    {
        new WormDamageableBehaviour()
    };

    private void OnDrawGizmos()
    {
        _patrolModel.OnDrawGizmos();
    }

    private void Awake()
    {
        base.Awake();
        _behaviourDebug = Behaviour.Born;
        base.SetFiniteBaseBehaviours(_finiteBaseBehaviours.Select(e => (EnemyFiniteBaseBehaviour) e ).ToList());
        base.SetInfiniteBaseBehaviours(_infiniteBaseBehaviours.Select(e => (EnemyInfiniteBaseBehaviours)e).ToList());
    }

    private void Start()
    {
        base.Start();
        GameManager.SetWorm(this);
    }

    private void OnDestroy()
    {
        GameManager.RemoveWorm(this);
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
        FollowingPlayer,
        Die
    }

}
