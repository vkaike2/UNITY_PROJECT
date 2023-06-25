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
        activePrefab.SetPositionInsideInventory(_itemParent);
        _itens.Add(activePrefab);
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
