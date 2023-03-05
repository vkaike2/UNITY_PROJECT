using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public abstract class EnemyInfiniteBaseBehaviours
{
    protected Hitbox _hitbox;
    protected GameManager _gameManager;

    public virtual void Start(Enemy worm)
    {
        _gameManager = worm.GameManager;
        _hitbox = worm.HitBox;
    }

    public abstract void Update();
}
