using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class ScriptableItem : ScriptableObject
{
    [Header("GENERAL")]
    [SerializeField]
    private string _name;
    [TextArea]
    [SerializeField]
    private string _description;

    public string Name => _name;
    public string Description => _description;


    [Header("INVENTORY INFO")]
    [SerializeField]
    private InventoryItemUI _prefabUI;

    public InventoryItemUI PrefabUI => _prefabUI;
}
