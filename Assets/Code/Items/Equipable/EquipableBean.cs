using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class EquipableBean : EquipableItemBase
{
    [Header("PREFABS")]
    [SerializeField]
    private FartProjectile _fartProjectile;

 
    protected override void EquipItem()
    {
        _playerStatus.Fart.Projectile.Set(_fartProjectile);
    }

    protected override void UnequipItem()
    {
        _playerStatus.Fart.Projectile.Reset();
    }
}
