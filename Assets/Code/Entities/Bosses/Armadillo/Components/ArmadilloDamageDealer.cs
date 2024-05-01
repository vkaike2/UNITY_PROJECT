using System;
using UnityEngine.Events;

public class ArmadilloDamageDealer : ImpactDamageDealer
{
    public OnRegisterShockwaveEvent OnRegisterShockwave { get; private set; } = new OnRegisterShockwaveEvent();

    private Armadillo _armadillo;

    private void Awake()
    {
        _armadillo = GetComponent<Armadillo>(); 
        OnRegisterShockwave.AddListener(RegisterShockwave);
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }

    private void RegisterShockwave(Shockwave shockwave)
    {
        Hitbox shockwaveHitbox = shockwave.Hitbox;
        shockwaveHitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnterShockwave);
    }

    private void OnHitboxEnterShockwave(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (targetHitbox == null) return;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_armadillo.Status.AtkDamage.Get(), myHitbox.GetInstanceID(), myHitbox.transform.position, "GunGaroo Bullet");

        Destroy(myHitbox.gameObject);
    }

    public class OnRegisterShockwaveEvent : UnityEvent<Shockwave> { };
}