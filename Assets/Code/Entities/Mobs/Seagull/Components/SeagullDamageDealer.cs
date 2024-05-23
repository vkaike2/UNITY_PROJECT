using UnityEngine.Events;

public class SeagullDamageDealer : ImpactDamageDealer
{
     private Seagull _seagull;
    public RegisterProjectile OnRegisterProjectileEvent { get; private set; } = new RegisterProjectile();

    private void Awake()
    {
        _seagull = GetComponent<Seagull>();

        OnRegisterProjectileEvent.AddListener(OnRegisterProjectile);
    }

    private void OnRegisterProjectile(SeagullProjectile projectile)
    {
        projectile.Hitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnterProjectile);
    }

    private void OnHitboxEnterProjectile(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (_seagull.CurrentBehaviour == Seagull.Behaviour.Die) return;
        if (targetHitbox == null) return;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_seagull.Status.AtkDamage.Get(), myHitbox.GetInstanceID(), myHitbox.transform.position, "Star Fish Projectile");
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_seagull.CurrentBehaviour == Seagull.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }

    public class RegisterProjectile : UnityEvent<SeagullProjectile> { }
}