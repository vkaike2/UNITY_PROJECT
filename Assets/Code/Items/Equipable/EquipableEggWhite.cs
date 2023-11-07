using System;
using UnityEngine;

[Serializable]
public class EquipableEggWhite : EquipableItemBase
{

    [Header("CONFIGURATIONS ADD")]
    [SerializeField]
    private float _addAreaPercentage = 0.1f;

    [Header("CONFIGURATIONS REDUCED")]
    [SerializeField]
    private float _reducedDamagePercentage = 0.05f;


    protected override void EquipItem()
    {
        _playerStatus.Fart.AreaOfEffect.IncreasePercentage(_addAreaPercentage);
        _playerStatus.Fart.Damage.ReducePercentage(_reducedDamagePercentage);
    }

    protected override void UnequipItem()
    {
        _playerStatus.Fart.AreaOfEffect.ReducePercentage(_addAreaPercentage);
        _playerStatus.Fart.Damage.IncreasePercentage(_reducedDamagePercentage);
    }
}
