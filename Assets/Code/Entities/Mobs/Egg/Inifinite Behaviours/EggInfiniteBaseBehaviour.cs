using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EggInfiniteBaseBehaviour : EnemyInfiniteBaseBehaviours
{
    protected Egg _egg;

    public override void Start(Enemy enemy)
    {
        _egg = (Egg)enemy;
        base.Start(enemy);
    }
}
