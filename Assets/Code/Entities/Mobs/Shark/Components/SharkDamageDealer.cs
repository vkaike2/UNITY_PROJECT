using System;
using UnityEngine;

public class SharkDamageDealer : ImpactDamageDealer
{
    [Header("MY COMPONENTS")]
    [SerializeField]
    private Hitbox _attackHitbox;

    private Shark _shark;

    private void Awake()
    {
        _shark = GetComponent<Shark>();

        _attackHitbox.OnHitboxTriggerEnter.AddListener(OnAttackHitboxEnter);
    }

    private void OnAttackHitboxEnter(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (_shark.CurrentBehaviour == Shark.Behaviour.Die) return;
        if (targetHitbox == null) return;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_shark.Status.AtkDamage.Get(), myHitbox.GetInstanceID(), myHitbox.transform.position, "Shark Bite");
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_shark.CurrentBehaviour == Shark.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }
}