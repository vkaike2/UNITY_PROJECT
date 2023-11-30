using System;
using UnityEngine;

[Serializable]
public class EquipableEggWhite : EquipableItemBase
{

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _addAreaOfEffect = 1f;
    [SerializeField]
    private int _increaseAmountOfParticle = 2;

    protected override void EquipItem()
    {
        _playerStatus.Fart.AreaOfEffect.Base.Add(_addAreaOfEffect);        
        _playerStatus.Fart.AmountOfParticle.Add(_increaseAmountOfParticle);        
    }

    protected override void UnequipItem()
    {
        _playerStatus.Fart.AmountOfParticle.Remove(_increaseAmountOfParticle);        
        _playerStatus.Fart.AreaOfEffect.Base.Remove(_addAreaOfEffect);
    }
}
