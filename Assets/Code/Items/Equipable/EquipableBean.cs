using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class EquipableBean : EquipableItemBase
{
    [Header("PREFABS")]
    [SerializeField]
    private FartProjectile _fartProjectile;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _addDuration = 2;
    [SerializeField]
    private float _increaseFartVelocity = 0.3f;

 
    protected override void EquipItem()
    {
        _playerStatus.Fart.Projectile.Set(_fartProjectile);
        _playerStatus.Fart.Duration.Base.Add(_addDuration);
        _playerStatus.Fart.Velocity.Increased.Add(_increaseFartVelocity);
    }

    protected override void UnequipItem()
    {
        _playerStatus.Fart.Duration.Base.Remove(_addDuration);
        _playerStatus.Fart.Projectile.Reset();
        _playerStatus.Fart.Velocity.Increased.Remove(_increaseFartVelocity);
    }
}
