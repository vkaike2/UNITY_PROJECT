using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class SpikeDamageDealer : ImpactDamageDealer
{

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }
}
