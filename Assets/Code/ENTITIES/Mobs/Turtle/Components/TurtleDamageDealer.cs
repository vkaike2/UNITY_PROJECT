using System;
using UnityEngine.Events;

public class TurtleDamageDealer : ImpactDamageDealer
{
    private Turtle _turtle;

    public RegisterProjectile OnRegisterProjectileEvent { get; private set; }

    private void Awake()
    {
        _turtle = GetComponent<Turtle>();

        OnRegisterProjectileEvent = new RegisterProjectile();
        OnRegisterProjectileEvent.AddListener(OnRegisterProjectile);
    }

    private void OnRegisterProjectile(TurtleProjectile projectile)
    {
        projectile.Hitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnterProjectile);
    }

    private void OnHitboxEnterProjectile(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (targetHitbox == null) return;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_turtle.Status.AtkDamage.Get(), myHitbox.GetInstanceID(), myHitbox.transform.position);

        Destroy(myHitbox.transform.parent.gameObject);
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_turtle.CurrentBehaviour == Turtle.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }

    public class RegisterProjectile : UnityEvent<TurtleProjectile> { }
}