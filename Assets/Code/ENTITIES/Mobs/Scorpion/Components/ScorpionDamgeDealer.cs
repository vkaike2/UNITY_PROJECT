using System;
using UnityEngine.Events;

public class ScorpionDamgeDealer : ImpactDamageDealer
{
    private Scorpion _scorpion;
    public RegisterProjectile OnRegisterProjectileEvent { get; private set; } = new RegisterProjectile();


    private void Awake()
    {
        _scorpion = GetComponent<Scorpion>();

        OnRegisterProjectileEvent.AddListener(OnRegisterProjectile);
    }

    private void OnRegisterProjectile(ScorpionProjectile projectile)
    {
        Hitbox projectileHitbox = projectile.GetComponent<Hitbox>();

        projectileHitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnterProjectile);
    }

    private void OnHitboxEnterProjectile(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (_scorpion.CurrentBehaviour == Scorpion.Behaviour.Die) return;
        if (targetHitbox == null) return;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_scorpion.Status.AtkDamage.Get(), myHitbox.GetInstanceID(), myHitbox.transform.position);
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_scorpion.CurrentBehaviour == Scorpion.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }

    public class RegisterProjectile : UnityEvent<ScorpionProjectile> { }
}