using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunGarooDamageDealer : ImpactDamageDealer
{
    public OnRegisterGunGarooBulletEvent OnRegisterBullet { get; private set; } = new OnRegisterGunGarooBulletEvent();

    private GunGaroo _gunGaroo;

    private void Awake()
    {
        _gunGaroo = GetComponent<GunGaroo>();
        this.OnRegisterBullet.AddListener(RegisterBullet);
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }

    private void RegisterBullet(GunGarooBullet bullet)
    {
        Hitbox bulletHitBox = bullet.GetComponent<Hitbox>();

        bulletHitBox.OnHitboxTriggerEnter.AddListener(OnHitboxEnterBullet);
    }

    private void OnHitboxEnterBullet(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (targetHitbox == null) return;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_gunGaroo.Status.AtkDamage.Get(), myHitbox.GetInstanceID(), myHitbox.transform.position, "GunGaroo Bullet");

        Destroy(myHitbox.gameObject);
    }

    public class OnRegisterGunGarooBulletEvent : UnityEvent<GunGarooBullet> { }
}
