using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenDamageDealer : ImpactDamageDealer
{
    [SerializeField]
    private Hitbox _atkHitbox;

    private EnemyStatus _enemyStatus;
    private Chicken _chicken;

    private void Awake()
    {
        _chicken = GetComponent<Chicken>();
        _enemyStatus = GetComponent<EnemyStatus>();
    }

    protected override void AfterStart()
    {
        _atkHitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnterAttack);
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_chicken.CurrentBehaviour == Chicken.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }

    private void OnHitboxEnterAttack(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (targetHitbox == null) return;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_enemyStatus.AtkDamage.Get(), myHitbox.GetInstanceID(), myHitbox.transform.position, "Chicken Attack");
    }
}
