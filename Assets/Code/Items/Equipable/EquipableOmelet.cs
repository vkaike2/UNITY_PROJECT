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
    [SerializeField]
    private int _addParticles = 3;

    protected override void EquipItem()
    {
        _playerStatus.Fart.Projectile.Set(_fartProjectile);
        _playerStatus.Fart.Duration.Base.Add(_addDuration);
        _playerStatus.Fart.AmountOfParticle.Add(_addParticles);
    }

    protected override void UnequipItem()
    {
        _playerStatus.Fart.Projectile.Reset();
        _playerStatus.Fart.Duration.Base.Remove(_addDuration);
        _playerStatus.Fart.AmountOfParticle.Remove(_addParticles);
    }
}
