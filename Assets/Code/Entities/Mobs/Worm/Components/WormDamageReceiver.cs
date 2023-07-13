using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WormDamageReceiver : DamageReceiver
{
    private Worm _worm;

    protected override void AfterAwake()
    {
        _worm = GetComponent<Worm>();
    }

    protected override void OnDie()
    {
        _worm.ChangeBehaviour(Worm.Behaviour.Die);
    }
}
