using System;
using UnityEngine;

[Serializable]
public class EquipableEggYolk : EquipableItemBase
{
    [Header("CONFIGURATIONS ADD")]
    [SerializeField]
    private float _reducedAreaOfEffect = 0.5f;
    [SerializeField]
    private int _addParticle = 1;
   

    protected override void EquipItem()
    {
        _playerStatus.Fart.AmountOfParticle.Add(_addParticle);
        _playerStatus.Fart.AreaOfEffect.Base.Remove(_reducedAreaOfEffect);
    }

    protected override void UnequipItem()
    {
        _playerStatus.Fart.AmountOfParticle.Remove(_addParticle);
        _playerStatus.Fart.AreaOfEffect.Base.Add(_reducedAreaOfEffect);
    }
}
