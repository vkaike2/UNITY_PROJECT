using Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chicken : Enemy
{
    [Header("debug")]
    [SerializeField]
    private Behaviour _behaviourDebug;

    [Header("my components")]
    [SerializeField]
    private ChickenAnimatorEvents _animatorEvents;

    [Header("my configuration")]
    [Space]
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private ChickenAnimatorModel _chickenAnimator;
    [Space]
    [SerializeField]
    private ChickenPatrolBehaviourModel _patrolModel;

    public float _JumpForce => _jumpForce;
    public ChickenAnimatorModel Animator => _chickenAnimator;
    public ChickenPatrolBehaviourModel PatrolModel => _patrolModel;

    //Finite Behaviours
    private readonly List<ChickenFiniteBaseBehaviour> _finiteBaseBehaviours = new List<ChickenFiniteBaseBehaviour>()
    {
        new ChickenPatrolBehaviour(),
        new ChickenFollowingPlayerBehaviour()
    };

    //Infinite Behaviours
    private readonly List<ChickenInfiniteBaseBehaviour> _infiniteBaseBehaviours = new List<ChickenInfiniteBaseBehaviour>()
    {
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
        base.SetInfiniteBaseBehaviours(_infiniteBaseBehaviours.Select(e => (EnemyInfiniteBaseBehaviours)e).ToList());
    }

    //called by animator events
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
        Atk_Melee,
    }
}
