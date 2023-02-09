using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.MANAGER;

public class Worm : MonoBehaviour
{
    //TODO = create a EntityManager to have the player
    //TODO = add ever enemy to it
    public Transform target;

    [Header("debug")]
    [SerializeField]
    private Behaviour _behaviourDebug;

    [Space]
    [Header("components")]
    [SerializeField]
    private Transform _rotationalTransform;
    [SerializeField]
    private Hitbox _hitbox;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    
    [Header("configuration")]
    [SerializeField]
    private float _movementSpeed = 3f;
    [Space]
    [SerializeField]
    private WormAnimatorModel _wormAnimator;
    [Space]
    [SerializeField]
    private WormPatrolBehaviourModel _patrolModel;
    [Space]
    [SerializeField]
    private WormDamageableBehavourModel _damageableModel;

    public Transform RotationalTransform => _rotationalTransform;
    public WormAnimatorModel Animator => _wormAnimator;
    public WormPatrolBehaviourModel PatrolModel => _patrolModel;
    public WormDamageableBehavourModel DamageableModel => _damageableModel;

    public Hitbox HitBox => _hitbox;
    public GameManager GameManager { get; private set; }
    public float MovementSpeed => _movementSpeed;
    public bool CanMove { get; set; }
    public Behaviour? CurrentBehaviour => _currentBehaviour?.Behaviour;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

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

    private WormFiniteBaseBehaviour _currentBehaviour;


    private void OnDrawGizmos()
    {
        _patrolModel.OnDrawGizmos();
    }

    private void Awake()
    {
        CanMove = false;
        _behaviourDebug = Behaviour.Born;

        GameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        foreach (var behaviour in _infiniteBaseBehaviours)
        {
            behaviour.Start(this);
        }

        foreach (var behaviour in _finiteBaseBehaviours)
        {
            behaviour.Start(this);
        }

        
    }

    private void FixedUpdate()
    {
        foreach (var behaviour in _infiniteBaseBehaviours)
        {
            behaviour.Update();
        }

        if (_currentBehaviour == null) return;

        _currentBehaviour.Update();
    }

    // called by animator events
    public void SetInitialBehaviour()
    {
        CanMove = true;
        this.ChangeBehaviour(Behaviour.Patrol);
    }

    public void ChangeBehaviour(Behaviour behaviour)
    {
        if (_currentBehaviour != null)
        {
            _currentBehaviour.OnExitBehaviour();
        }

        _behaviourDebug = behaviour;
        _currentBehaviour = _finiteBaseBehaviours.FirstOrDefault(e => e.Behaviour == behaviour);

        _currentBehaviour.OnEnterBehaviour();
    }

    public enum Behaviour
    {
        Born,
        Patrol,
        FollowingPlayer,
        Die
    }

}
