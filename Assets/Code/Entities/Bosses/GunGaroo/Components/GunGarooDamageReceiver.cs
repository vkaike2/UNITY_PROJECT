using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGarooDamageReceiver : DamageReceiver
{
    private GunGaroo _gunGaroo;

    protected override void AfterAwake()
    {
        base.AfterAwake();
        _gunGaroo = GetComponent<GunGaroo>();
    }

    protected override void OnDie(string damageSource)
    {
        _gunGaroo.ChangeBehaviour(GunGaroo.Behaviour.Death);
    }
}
