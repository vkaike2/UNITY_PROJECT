using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RatBaseBehaviour : EnemyBaseBehaviour
{
    public abstract Rat.Behaviour Behaviour { get; }

    protected Rat _rat;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _rat = (Rat)enemy;
    }
}
