using System;
using UnityEngine;

[Serializable]
public class EquipableEggYolk : EquipableItemBase
{
    [Header("CONFIGURATIONS ADD")]
    [SerializeField]
    private float _reducedAreaOfEffect = 0.5f;
    [SerializeField]
    private float _addBaseDamage = 0.1f;
   

    protected override void EquipItem()
    {
        _playerStatus.Fart.Damage.Base.Add(_addBaseDamage);
        _playerStatus.Fart.AreaOfEffect.Base.Remove(_reducedAreaOfEffect);
    }

    protected override void UnequipItem()
    {
        _playerStatus.Fart.Damage.Base.Remove(_addBaseDamage);
        _playerStatus.Fart.AreaOfEffect.Base.Add(_reducedAreaOfEffect);
    }
}
