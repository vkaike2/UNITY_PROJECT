using Assets.Code.MANAGER;
using Pathfinding;
using System;
using UnityEngine;

public abstract class WormFiniteBaseBehaviour
{
    protected Worm _worm;
    protected Rigidbody2D _rigidbody2D;
    protected MySeeker _mySeeker;
    protected GameManager _gameManager;

    public abstract Worm.Behaviour Behaviour { get; }

    public virtual void Start(Worm worm)
    {
        _worm = worm;
        _gameManager = worm.GameManager;
        _mySeeker = worm.GetComponent<MySeeker>();
        _rigidbody2D = worm.GetComponent<Rigidbody2D>();
    }

    public abstract void Update();

    public abstract void OnEnterBehaviour();
    public abstract void OnExitBehaviour();
}
