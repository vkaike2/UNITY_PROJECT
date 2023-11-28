public class SpikeDamageDealer : ImpactDamageDealer
{
    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }
}
