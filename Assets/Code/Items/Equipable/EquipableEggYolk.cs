using System;
using UnityEngine;

[Serializable]
public class EquipableEggYolk : EquipableItemBase
{

    [Header("CONFIGURATIONS ADD")]
    [SerializeField]
    private float _addDamagePercentage = 0.1f;

    [Header("CONFIGURATIONS REDUCED")]
    [SerializeField]
    private float _reducedAreaPercentage = 0.05f;


    protected override void EquipItem()
    {
        _playerStatus.Fart.AreaOfEffect.ReducePercentage(_reducedAreaPercentage);
        _playerStatus.Fart.Damage.IncreasePercentage(_addDamagePercentage);
    }

    protected override void UnequipItem()
    {
        _playerStatus.Fart.AreaOfEffect.IncreasePercentage(_reducedAreaPercentage);
        _playerStatus.Fart.Damage.ReducePercentage(_addDamagePercentage);
    }
}
