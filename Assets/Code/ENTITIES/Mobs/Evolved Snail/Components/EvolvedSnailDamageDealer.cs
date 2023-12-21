using UnityEngine.Events;

public class EvolvedSnailDamageDealer : ImpactDamageDealer
{
    private EvolvedSnail _evolvedSnail;
    public RegisterProjectile OnRegisterProjectileEvent { get; private set; } = new RegisterProjectile();

    private void Awake()
    {
        _evolvedSnail = GetComponent<EvolvedSnail>();

        OnRegisterProjectileEvent.AddListener(OnRegisterProjectile);
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_evolvedSnail.CurrentBehaviour == EvolvedSnail.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }

    private void OnRegisterProjectile(EvolvedSnailProjectile projectile)
    {
        Hitbox projectileHitbox = projectile.GetComponent<Hitbox>();

        projectileHitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnterProjectile);
    }

     private void OnHitboxEnterProjectile(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (_evolvedSnail.CurrentBehaviour == EvolvedSnail.Behaviour.Die) return;
        if (targetHitbox == null) return;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_evolvedSnail.Status.AtkDamage.Get(), myHitbox.GetInstanceID(), myHitbox.transform.position);
    }

    public class RegisterProjectile : UnityEvent<EvolvedSnailProjectile> { }
}