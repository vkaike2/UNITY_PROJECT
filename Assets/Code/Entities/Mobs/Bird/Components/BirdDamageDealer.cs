public class BirdDamageDealer : ImpactDamageDealer
{
    private Bird _bird;

    private void Awake()
    {
        _bird = GetComponent<Bird>();
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_bird.CurrentBehaviour == Bird.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }
}
