using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class EquipableMilk : EquipableItemBase
{
    [Header("CONFIGURATION")]
    [SerializeField]
    private float _reudcedCdwToPoop = 0.1f;

    protected override void EquipItem()
    {
        _playerStatus.Poop.CdwToPoop.Increased.Remove(_reudcedCdwToPoop);
    }

    protected override void UnequipItem()
    {
        _playerStatus.Poop.CdwToPoop.Increased.Add(_reudcedCdwToPoop);
    }
}