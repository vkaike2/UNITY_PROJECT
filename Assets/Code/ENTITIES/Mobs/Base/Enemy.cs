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

    protected abstract List<EnemyBaseBehaviour> FiniteBaseBehaviours { get; }
    protected EnemyBaseBehaviour _currentFiniteBehaviour;
    protected EnemyStatus _status;

    private void Awake()
    {
        _status = GetComponent<EnemyStatus>();
        GameManager = GameObject.FindObjectOfType<GameManager>();
        CanMove = false;
        AfterAwake();
    }

    private void Start()
    {
        BeforeStart();
        foreach (var behaviour in FiniteBaseBehaviours)
        {
            behaviour.Start(this);
        }
        AfterStart();
    }

    protected void FixedUpdate()
    {
        if (_currentFiniteBehaviour == null) return;
        _currentFiniteBehaviour.Update();
    }

    protected virtual void AfterAwake() { }
    protected virtual void BeforeStart() { }
    protected virtual void AfterStart() { }
}
