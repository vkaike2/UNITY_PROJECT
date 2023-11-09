using System;
using UnityEngine;

[Serializable]
public class EquipableSteak : EquipableItemBase
{
    [Header("PREFABS")]
    [SerializeField]
    private PoopProjectile _poopProjectile;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _reducedDamageMultiplier = 0.5f;

    protected override void EquipItem()
    {
        _playerStatus.Poop.Projectile.Set(_poopProjectile);

        _playerStatus.Poop.Damage.Multiplier.Remove(_reducedDamageMultiplier);
    }

    protected override void UnequipItem()
    {
        _playerStatus.Poop.Projectile.Reset();

        _playerStatus.Poop.Damage.Multiplier.Add(_reducedDamageMultiplier);
    }
}
