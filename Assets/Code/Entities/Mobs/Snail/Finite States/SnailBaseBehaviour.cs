using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SnailBaseBehaviour : EnemyBaseBehaviour
{
    public abstract Snail.Behaviour Behaviour { get; }

    protected Snail _snail;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _snail = (Snail)enemy;
    }
}
