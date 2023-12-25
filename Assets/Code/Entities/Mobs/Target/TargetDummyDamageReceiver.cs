using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummyDamageReceiver : DamageReceiver
{

    protected override void OnDie(string damageSource)
    {
        _status.Health.ResetToDefault();
    }
}
