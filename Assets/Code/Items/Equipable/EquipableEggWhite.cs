using System;
using UnityEngine;

[Serializable]
public class EquipableEggWhite : EquipableItemBase
{

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _addAreaOfEffect = 1f;

    protected override void EquipItem()
    {
        _playerStatus.Fart.AreaOfEffect.Base.Add(_addAreaOfEffect);
        
        //_playerStatus.Fart.BaseDamage.ReducePercentage(_reducedDamagePercentage);
    }

    protected override void UnequipItem()
    {
        _playerStatus.Fart.AreaOfEffect.Base.Remove(_addAreaOfEffect);

        //_playerStatus.Fart.BaseDamage.IncreasePercentage(_reducedDamagePercentage);
    }
}
