using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class EquipableMilk : EquipableItemBase
{
    [Header("CONFIGURATION")]
    [SerializeField]
    private float _reducedFlatCdw = 0.3f;

    protected override void EquipItem()
    {
        _playerStatus.Poop.CdwToPoop.Base.Remove(_reducedFlatCdw);
    }

    protected override void UnequipItem()
    {
        _playerStatus.Poop.CdwToPoop.Base.Add(_reducedFlatCdw);
    }
}