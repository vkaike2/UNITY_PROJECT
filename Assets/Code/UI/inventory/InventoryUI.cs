using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEngine;
using static InventoryData;

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

    private readonly List<InventoryItemUI> _itens = new List<InventoryItemUI>();
    private Animator _animator;
    private UIEventManager _uiEventManager;
    private bool _isOpen = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _equipmentInfo.Awake();
        _inventoyInfo.Awake();
    }

    private void Start()
    {
        _uiEventManager = GameObject.FindObjectOfType<UIEventManager>();
        _uiEventManager.OnToggleInventoryOpen.AddListener(OnToggleInventoryOpen);

        _uiEventManager.OnInventoryChange.AddListener(OnInventoryChange);
        StartCoroutine(WaitUntilEndOfFrame());
    }

    IEnumerator WaitUntilEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        UpdatePlayerInventory();
    }

    #region PUBLIC METHODS
    public void AddItem(InventoryItemUI itemUI)
    {
        List<InventorySlotUI> slotsUnderItem = itemUI.SetPositionInsideInventory(_itemParent);

        InternalAddItem(slotsUnderItem, itemUI);
    }

    public void RemoveItem(Guid itemId)
    {
        InventoryItemUI item = _itens.FirstOrDefault(e => e.ItemData.Id == itemId);
        _itens.Remove(item);

        List<InventorySlotUI> slotsUnderItem = item.GetEveryInventorySlotUnderItem();

        foreach (var slot in slotsUnderItem)
        {
            slot.RemoveItem();
        }
    }

    public void UpdatePlayerInventory() => _uiEventManager.OnInventoryChange.Invoke(GenerateInventoryData(), EventSentBy.UI);
    #endregion

    private void OnToggleInventoryOpen()
    {
        if (_isOpen)
        {
            _animator.Play(MyAnimations.Close.ToString());
        }
        else
        {
            _animator.Play(MyAnimations.Open.ToString());
        }

        _isOpen = !_isOpen;
    }

    private InventoryData GenerateInventoryData()
    {
        InventoryData data = new InventoryData(InventoryData.SlotType.Inventory);

        data.Slots = _inventoyInfo.Slots
            .Select(e => new InventoryData.Slot()
            {
                Coordinate = e.Coordinate,
                HasItem = e.HasItem,
                ItemId = e.ItemUI != null ? e.ItemUI.ItemData.Id : null,
            })
            .ToList();
        data.Itens = _itens.Select(e => e.ItemData).ToList();
        return data;
    }

    private void OnInventoryChange(InventoryData inventoryData, EventSentBy sentBy)
    {
        if (sentBy == EventSentBy.UI) return;

        if (inventoryData.Type == SlotType.Inventory)
        {
            UpdateInventoryFromEvent(inventoryData);
        }
        else
        {
            UpdateEquipFromEvent(inventoryData);
        }

    }

    private void UpdateInventoryFromEvent(InventoryData inventoryData)
    {
        CleanInventory();
        foreach (ItemData item in inventoryData.Itens)
        {
            List<Vector2> coordinatesUnderItem = inventoryData.Slots.Where(e => e.ItemId == item.Id)
                .Select(e => e.Coordinate)
                .ToList();

            List<InventorySlotUI> slotsUnderItem = _inventoyInfo.Slots
                .Where(e => coordinatesUnderItem.Contains(e.Coordinate))
                .ToList();

            List<Vector2> slotPositions = slotsUnderItem
                .Select(e => (Vector2)e.transform.position)
                .ToList();

            Vector2 itemPosition = slotPositions.CalculateMiddlePosition();

            InventoryItemUI itemUI = Instantiate(item.Item.PrefabUI, this._itemParent);
            itemUI.ItemData = item;
            itemUI.transform.position = itemPosition;

            this.InternalAddItem(slotsUnderItem, itemUI, false);
        }
    }

    private void UpdateEquipFromEvent(InventoryData equipData)
    {

    }

    private void InternalAddItem(List<InventorySlotUI> slotsUnderItem, InventoryItemUI itemUI, bool updatePlayer = true)
    {
        foreach (InventorySlotUI slot in slotsUnderItem)
        {
            slot.AddItem(itemUI, updatePlayer);
        }

        _itens.Add(itemUI);
    }

    private void CleanInventory()
    {
        foreach (InventoryItemUI item in _itens)
        {
            Destroy(item.gameObject);
        }
        _itens.Clear();

        foreach (InventorySlotUI slot in _inventoyInfo.Slots)
        {
            if (!slot.HasItem) continue;

            slot.RemoveItem(false);
        }
    }


    private enum MyAnimations
    {
        Open, Close
    }

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
