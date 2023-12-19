using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatDamageDealer : ImpactDamageDealer
{
    private Bat _bat;

    private void Awake()
    {
        _bat = GetComponent<Bat>();
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_bat.CurrentBehaviour == Bat.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }
}
