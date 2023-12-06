using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class UsableChocolateCoin : UsableItemBase
{
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _amountOfHealth = 5f;

    protected override void UseItem()
    {         
        _playerDamageReceiver.AddHealth(_amountOfHealth);
    }
}
