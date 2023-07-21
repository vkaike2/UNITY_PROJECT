using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatDamageDealer : ImpactDamageDealer
{
    private Rat _rat;

    private void Awake()
    {
        _rat = GetComponent<Rat>();
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_rat.CurrentBehaviour == Rat.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }
}
