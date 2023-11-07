using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummyDamageReceiver : DamageReceiver
{

    protected override void OnDie()
    {
        _status.Health.ResetToDefault();
    }
}
