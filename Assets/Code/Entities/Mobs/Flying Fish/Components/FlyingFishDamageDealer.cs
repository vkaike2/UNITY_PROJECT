public class FlyingFishDamageDealer : ImpactDamageDealer
{
    private FlyingFish _flyingFish;

    private void Awake()
    {
        _flyingFish = GetComponent<FlyingFish>();
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_flyingFish.CurrentBehaviour == FlyingFish.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }
}