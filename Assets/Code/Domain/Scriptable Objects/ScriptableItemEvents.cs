﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Events/Item")]
public class ScriptableItemEvents : ScriptableObject
{
    [Header("EQUIPABLE ITENS")]
    [Header("-> FART - MAJOR")]
    [SerializeField]
    private EquipableBean _bean;
    [SerializeField]
    private EquipableOmelet _omelet;

    [Header("-> FART - MINOR")]
    [SerializeField]
    private EquipableEggWhite _eggWhite;
    [SerializeField]
    private EquipableEggYolk _eggYolk;

    [Header("-> POOP - MAJOR")]
    [SerializeField]
    private EquipablePepper _pepper;
    [SerializeField]
    private EquipableSteak _steak;

    [Header("-> POOP - MINOR")]
    [SerializeField]
    private EquipableMilk _milk;
    [SerializeField]
    private EquipableBroccoli _broccoli;

    public OnUseJamEvent OnUseGoldenJam { get; private set; } = new OnUseJamEvent();

    public OnEquipItemEvent OnEquipItem { get; private set; } = new OnEquipItemEvent();
    public OnUnequipItemEvent OnUnequipItem { get; private set; } = new OnUnequipItemEvent();

    public void InitializeEvent(GameManager gameManage)
    {
        List<EquipableItemBase> _equipableItens = new List<EquipableItemBase>()
        {
            _bean,
            _omelet,
            _eggWhite,
            _eggYolk,
            _pepper,
            _steak,
            _milk,
            _broccoli
        };

        foreach (var equipment in _equipableItens)
        {
            equipment.Initialize(this,gameManage);
        }
    }


    /// <summary>
    /// int => gameInstanceId
    /// </summary>
    public class OnUseJamEvent : UnityEvent<int> { }
    public class OnEquipItemEvent : UnityEvent<ScriptableItem> { }
    public class OnUnequipItemEvent : UnityEvent<ScriptableItem> { }
}