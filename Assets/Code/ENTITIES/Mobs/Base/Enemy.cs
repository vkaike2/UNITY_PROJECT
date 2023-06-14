using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Transform _rotationalTransform;
    [SerializeField]
    private Hitbox _hitbox;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    public Transform RotationalTransform => _rotationalTransform;
    public Hitbox HitBox => _hitbox;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public GameManager GameManager { get; private set; }
    public bool CanMove { get; set; }
    public EnemyStatus Status => _status;

    private List<EnemyFiniteBaseBehaviour> _finiteBaseBehaviours;
    private List<EnemyInfiniteBaseBehaviours> _infiniteBaseBehaviours;

    protected EnemyFiniteBaseBehaviour _currentFiniteBehaviour;

    protected EnemyStatus _status;

    protected void Awake()
    {
        _status = GetComponent<EnemyStatus>();
        GameManager = GameObject.FindObjectOfType<GameManager>();
        CanMove = false;
    }

    protected virtual void SetFiniteBaseBehaviours(List<EnemyFiniteBaseBehaviour> finiteBaseBehaviours)
    {
        _finiteBaseBehaviours = finiteBaseBehaviours;
    }

    protected virtual void SetInfiniteBaseBehaviours(List<EnemyInfiniteBaseBehaviours> infiniteBaseBehaviours)
    {
        _infiniteBaseBehaviours = infiniteBaseBehaviours;
    }

    protected void Start()
    {
        if (_finiteBaseBehaviours == null) throw new NotImplementedException(nameof(_finiteBaseBehaviours));
        if (_infiniteBaseBehaviours == null) throw new NotImplementedException(nameof(_infiniteBaseBehaviours));
        
        foreach (var behaviour in _infiniteBaseBehaviours)
        {
            behaviour.Start(this);
        }

        foreach (var behaviour in _finiteBaseBehaviours)
        {
            behaviour.Start(this);
        }
    }

    protected void FixedUpdate()
    {
        foreach (var behaviour in _infiniteBaseBehaviours)
        {
            behaviour.Update();
        }

        if (_currentFiniteBehaviour == null) return;
        _currentFiniteBehaviour.Update();
    }
}
