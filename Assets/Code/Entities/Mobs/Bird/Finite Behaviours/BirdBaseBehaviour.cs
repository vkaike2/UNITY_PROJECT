using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BirdBaseBehaviour : EnemyBaseBehaviour
{
    public abstract Bird.Behaviour Behaviour { get; }

    protected Bird _bird;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _bird = (Bird)enemy;
    }
}
