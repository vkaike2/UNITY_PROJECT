using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class EquipableBroccoli : EquipableItemBase
{
    [Header("CONFIGURATION")]
    [SerializeField]
    private float _addFlatDamage = 0.2f;

    protected override void EquipItem()
    {
        _playerStatus.Poop.Damage.Base.Add(_addFlatDamage);
    }

    protected override void UnequipItem()
    {
        _playerStatus.Poop.Damage.Base.Remove(_addFlatDamage);
    }
}
