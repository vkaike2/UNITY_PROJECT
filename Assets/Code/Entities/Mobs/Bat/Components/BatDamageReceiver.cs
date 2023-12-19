using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatDamageReceiver : DamageReceiver
{
    private Bat _bat;

    protected override void AfterAwake()
    {
        _bat = GetComponent<Bat>();
    }

    protected override void OnDie()
    {
        _bat.ChangeBehaviour(Bat.Behaviour.Die);
    }
}
