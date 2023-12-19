using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailDamageDealer : ImpactDamageDealer
{
    private Snail _snail;

    private void Awake()
    {
        _snail = GetComponent<Snail>();
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_snail.CurrentBehaviour == Snail.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }
}
