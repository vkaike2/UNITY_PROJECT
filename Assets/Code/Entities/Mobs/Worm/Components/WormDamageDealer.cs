using System.Collections;
using UnityEngine;

public class WormDamageDealer : ImpactDamageDealer
{
    private Worm _worm;

    private void Awake()
    {
        _worm = GetComponent<Worm>();
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_worm.CurrentBehaviour == Worm.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }
}
