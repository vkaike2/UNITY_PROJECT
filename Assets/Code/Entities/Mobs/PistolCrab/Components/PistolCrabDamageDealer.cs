using System;
using UnityEngine.Events;

public class PistolCrabDamageDealer : ImpactDamageDealer
{
    private PistolCrab _pistolCrab;

    public RegisterProjectile OnRegisterProjectileEvent { get; private set; } = new RegisterProjectile();

    private void Awake()
    {
        _pistolCrab = GetComponent<PistolCrab>();

        OnRegisterProjectileEvent.AddListener(OnRegisterProjectile);
    }

    private void OnRegisterProjectile(PistolCrabProjectile projectile)
    {
        Hitbox projectileHitbox = projectile.GetComponent<Hitbox>();

        projectileHitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnterProjectile);
    }

    private void OnHitboxEnterProjectile(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (_pistolCrab.CurrentBehaviour == PistolCrab.Behaviour.Die) return;
        if (targetHitbox == null) return;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_pistolCrab.Status.AtkDamage.Get(), myHitbox.GetInstanceID(), myHitbox.transform.position, "Pistol Crab Projectile");
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_pistolCrab.CurrentBehaviour == PistolCrab.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }

    public class RegisterProjectile : UnityEvent<PistolCrabProjectile> { }
}