using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private Transform _itemParent;

    [Header("SLOTS")]
    [SerializeField]
    SlotInformation _equipmentInfo;
    [SerializeField]
    SlotInformation _inventoyInfo;

    private List<InventoryItemUI> _itens = new List<InventoryItemUI>();

    private void Awake()
    {
        _equipmentInfo.Awake();
        _inventoyInfo.Awake();
    }

    #region PUBLIC METHODS
    public void AddItem(InventoryItemUI activePrefab)
    {
        List<InventorySlotUI> slotsUnderItem = activePrefab.AddToInventory(_itemParent, this);

        foreach (InventorySlotUI slot in slotsUnderItem)
        {
            slot.AddItem(activePrefab);
        }

        _itens.Add(activePrefab);
    }

    public void RemoveItem(Guid itemId)
    {
        foreach (var i in _itens)
        {
           var sl = i.GetEveryInventorySlotUnderItem().First();
           Debug.Log($"item: {i.ItemData.Id} \t slot: {sl.name}");

        }

        InventoryItemUI item = _itens.FirstOrDefault(e => e.ItemData.Id == itemId);
        _itens.Remove(item);
        Debug.Log($"remove item {item.ItemData.Id}");


        List<InventorySlotUI> slotsUnderItem = item.GetEveryInventorySlotUnderItem();

        foreach (var slot in slotsUnderItem)
        {
            slot.RemoveItem();
        }
    }

    #endregion

    [Serializable]
    public class SlotInformation
    {
        [SerializeField]
        private Transform _slotParent;

        public List<InventorySlotUI> Slots { get; set; }

        public void Awake()
        {
            Slots = _slotParent.GetComponentsInChildren<InventorySlotUI>().ToList();
        }
    }
}
