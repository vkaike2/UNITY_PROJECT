using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDamageReceiver : DamageReceiver
{
    private Egg _egg;

    protected override void AfterAwake()
    {
        _egg = GetComponent<Egg>();
    }

    protected override void OnDie()
    {
        _egg.ChangeBehaviour(Egg.Behaviour.Die);
    }
}
