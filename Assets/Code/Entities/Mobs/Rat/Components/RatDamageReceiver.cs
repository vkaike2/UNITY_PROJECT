using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatDamageReceiver : DamageReceiver
{
    private Rat _rat;

    protected override void AfterAwake()
    {
        _rat = GetComponent<Rat>();
    }

    protected override void OnDie()
    {
        _rat.ChangeBehaviour(Rat.Behaviour.Die);
    }
}
