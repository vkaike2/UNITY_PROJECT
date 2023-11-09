using System;
using UnityEngine;

[Serializable]
public class EquipableOmelet : EquipableItemBase
{
    [Header("PREFABS")]
    [SerializeField]
    private FartProjectile _fartProjectile;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _addDuration = 1f;

    protected override void EquipItem()
    {
        _playerStatus.Fart.Projectile.Set(_fartProjectile);
        _playerStatus.Fart.Duration.Base.Add(_addDuration);
    }

    protected override void UnequipItem()
    {
        _playerStatus.Fart.Projectile.Reset();
        _playerStatus.Fart.Duration.Base.Remove(_addDuration);
    }
}
