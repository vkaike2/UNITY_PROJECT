using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class EquipableSteak : EquipableItemBase
{
    [Header("PREFABS")]
    [SerializeField]
    private PoopProjectile _poopProjectile;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _damageMultiplier = 0.5f;

    protected override void EquipItem()
    {
        _playerStatus.Poop.Projectile.Set(_poopProjectile);
        _playerStatus.Poop.DamageMultiplier.Set(_damageMultiplier);
    }

    protected override void UnequipItem()
    {
        _playerStatus.Poop.Projectile.Reset();
        _playerStatus.Poop.DamageMultiplier.ResetToDefault();
    }
}
