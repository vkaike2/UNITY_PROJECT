using UnityEngine.Events;

public class StarFishDamageDealer : ImpactDamageDealer
{
    private StarFish _starFish;
    public RegisterProjectile OnRegisterProjectileEvent { get; private set; } = new RegisterProjectile();

    private void Awake()
    {
        _starFish = GetComponent<StarFish>();

        OnRegisterProjectileEvent.AddListener(OnRegisterProjectile);
    }

    private void OnRegisterProjectile(StarFishProjectile projectile)
    {
        Hitbox projectileHitbox = projectile.GetComponent<Hitbox>();

        projectileHitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnterProjectile);
    }

    private void OnHitboxEnterProjectile(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (_starFish.CurrentBehaviour == StarFish.Behaviour.Die) return;
        if (targetHitbox == null) return;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_starFish.Status.AtkDamage.Get(), myHitbox.GetInstanceID(), myHitbox.transform.position, "Scorpion Projectile");
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_starFish.CurrentBehaviour == StarFish.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }

    public class RegisterProjectile : UnityEvent<StarFishProjectile> { }
}