using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenDamageReceiver : DamageReceiver
{
    private Chicken _chicken;

    protected override void AfterAwake()
    {
        _chicken = GetComponent<Chicken>();
    }

    protected override void OnDie()
    {
        _chicken.ChangeBehaviour(Chicken.Behaviour.Die);
    }
}
