using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Transactions;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("ITEM EVENT")]
    [SerializeField]
    private ScriptableItemEvents _itemEvents;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private Transform _itemParent;

    [Header("SLOTS")]
    [SerializeField]
    SlotInformation _equipmentInfo;
    [SerializeField]
    SlotInformation _inventoyInfo;

    public bool IsOpen { get; private set; }

    private readonly List<InventoryItemUI> _itens = new List<InventoryItemUI>();
    private Animator _animator;
    private GameManager _gameManager;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _equipmentInfo.Awake();
        _inventoyInfo.Awake();
    }
    private void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();

        _gameManager.SetInventory(this);
        SetupEvents();
    }

    IEnumerator WaitUntilEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        UpdatePlayerInventory();
        UpdatePlayerEquipInventory();
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

        List<InventorySlotUI> slotsUnderItem = _inventoyInfo.Slots.Where(e => e.HasItem && e.ItemUI.ItemData.Id == item.ItemData.Id).ToList();
        if (!slotsUnderItem.Any())
        {
            slotsUnderItem = _equipmentInfo.Slots.Where(e => e.HasItem && e.ItemUI.ItemData.Id == item.ItemData.Id).ToList();
        }

        List<SlotActionWrapper> slotActions = slotsUnderItem
            .Select(e => new SlotActionWrapper(e, e.RemoveItem()))
            .ToList();

        StartCoroutine(WaitForActionsThenDo(slotActions, () =>
        {
            UpdatePlayerEvent(slotsUnderItem);
        }));
    }

    public void UpdatePlayerInventory() => UIEventManager.instance.OnInventoryChange.Invoke(GenerateInventoryData(SlotType.Inventory, _inventoyInfo), EventSentBy.UI);

    public void UpdatePlayerEquipInventory() => UIEventManager.instance.OnInventoryChange.Invoke(GenerateInventoryData(SlotType.Equip, _equipmentInfo), EventSentBy.UI);

    #endregion

    private void SetupEvents()
    {
        UIEventManager.instance.OnToggleInventoryOpen.AddListener(OnToggleInventoryOpen);
        UIEventManager.instance.OnInventoryChange.AddListener(OnInventoryChange);

        UIEventManager.instance.OnInventoryRemoveEquipment.AddListener(OnInventoryRemoveEquipment);

        _itemEvents.OnUseGoldenJam.AddListener(OnUseGoldenJam);

        StartCoroutine(WaitUntilEndOfFrame());
    }

    private void OnToggleInventoryOpen()
    {
        if (IsOpen)
        {
            //_gameManager.PauseGame(false);
            _animator.Play(MyAnimations.Close.ToString());
        }
        else
        {
            //StartCoroutine(WaitThenPause());
            _animator.Play(MyAnimations.Open.ToString());
        }


        IsOpen = !IsOpen;
    }

    private InventoryData GenerateInventoryData(SlotType type, SlotInformation slotInformation)
    {
        InventoryData data = new InventoryData(type);

        data.Slots = slotInformation.Slots
            .Where(e => e.Type == type)
            .Select(e => new InventoryData.Slot()
            {
                Coordinate = e.Coordinate,
                HasItem = e.HasItem,
                ItemId = e.ItemUI != null ? e.ItemUI.ItemData.Id : null,
                IsAvailable = e.IsAvalialbe
            })
            .ToList();

        data.Itens = _itens.Where(e => data.Slots.Select(i => i.ItemId).Contains(e.ItemData.Id)).Select(e => e.ItemData).ToList();
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
        CleanInventory(SlotType.Inventory, _inventoyInfo);
        UpdateGeneralInventoryEvent(inventoryData, _inventoyInfo);
    }

    private void UpdateEquipFromEvent(InventoryData inventoryData)
    {
        CleanInventory(SlotType.Equip, _equipmentInfo);
        UpdateGeneralInventoryEvent(inventoryData, _equipmentInfo);
    }

    private void UpdateGeneralInventoryEvent(InventoryData inventoryData, SlotInformation slotInformation)
    {

        foreach (ItemData item in inventoryData.Itens)
        {
            List<Vector2> coordinatesUnderItem = inventoryData.Slots.Where(e => e.ItemId == item.Id)
                .Select(e => e.Coordinate)
                .ToList();

            List<InventorySlotUI> slotsUnderItem = slotInformation.Slots
                .Where(e => coordinatesUnderItem.Contains(e.Coordinate))
                .ToList();

            if (slotsUnderItem.Count == 0)
            {
                continue;
            }

            List<Vector2> slotPositions = slotsUnderItem
                .Select(e => (Vector2)e.transform.position)
                .ToList();

            Vector2 itemPosition = slotPositions.CalculateMiddlePosition();

            InventoryItemUI itemUI = Instantiate(item.Item.PrefabUI, this._itemParent);
            itemUI.ItemData = item;
            itemUI.transform.position = itemPosition;
            itemUI.SlotType = inventoryData.Type;

            this.InternalAddItem(slotsUnderItem, itemUI, false);
        }
    }
    //1608 187348
    private void InternalAddItem(List<InventorySlotUI> slotsUnderItem, InventoryItemUI itemUI, bool updatePlayer = true)
    {
        bool canEquipItem = true;
        if (slotsUnderItem.Any(e => e.Type == SlotType.Equip))
        {
            canEquipItem = _gameManager.PlayerInventory.CanEquipItem(itemUI.ItemData);
        }

        List<SlotActionWrapper> slotActions = slotsUnderItem
            .Select(e => new SlotActionWrapper(e, e.AddItem(itemUI, canEquipItem)))
            .ToList();

        StartCoroutine(WaitForActionsThenDo(slotActions, () =>
        {
            _itens.Add(itemUI);

            if (updatePlayer)
            {
                UpdatePlayerEvent(slotsUnderItem);
            }
        }));
    }

    private void UpdatePlayerEvent(List<InventorySlotUI> slotsUnderItem)
    {
        if (slotsUnderItem.Any(e => e.Type == SlotType.Equip))
        {
            UpdatePlayerEquipInventory();
        }
        else
        {
            UpdatePlayerInventory();
        }
    }

    private void OnInventoryRemoveEquipment()
    {
        List<InventorySlotUI> slotsWithError = _equipmentInfo.Slots.Where(e => e.HasError).ToList();
        if (!slotsWithError.Any()) return;

        foreach (var itemData in slotsWithError.Select(e => e.ItemUI.ItemData).Distinct().ToList())
        {
            if (!_gameManager.PlayerInventory.CanEquipItem(itemData)) continue;

            List<InventorySlotUI> slotsUnderItem = slotsWithError.Where(e => e.ItemUI.ItemData.Id == itemData.Id).ToList();
            foreach (var slot in slotsUnderItem)
            {
                slot.FixEquipment();
            }
        }

    }

    private void CleanInventory(SlotType slotType, SlotInformation slotInformation)
    {
        List<InventoryItemUI> specificItems = _itens.Where(e => e.SlotType == slotType).ToList();

        _itens.RemoveAll(e => specificItems.Select(i => i.ItemData.Id).ToList().Contains(e.ItemData.Id));

        foreach (InventoryItemUI item in specificItems)
        {
            Destroy(item.gameObject);
        }

        foreach (InventorySlotUI slot in slotInformation.Slots.Where(e => e.Type == slotType))
        {
            if (!slot.HasItem) continue;

            slot.RemoveItem();
        }
    }

    #region EVENT LISTENERS FOR ITENS
    private void OnUseGoldenJam(int id)
    {
        List<InventorySlotUI> unavailableSlots = _equipmentInfo.Slots
            .Where(e => e.IsAvalialbe == false)
            .OrderBy(e => e.Coordinate.x)
            .ToList();

        if (!unavailableSlots.Any()) return;

        float columnIdex = unavailableSlots.FirstOrDefault().Coordinate.x;

        List<InventorySlotUI> columnBeingActivatedSlots = unavailableSlots.Where(e => e.Coordinate.x == columnIdex).ToList();

        foreach (var slot in columnBeingActivatedSlots)
        {
            slot.ToggleAvailability(true);
        }
    }
    #endregion

    private IEnumerator WaitForActionsThenDo(List<SlotActionWrapper> slotActions, Action doAfter)
    {
        while (!slotActions.Any(e => e.InventorySlot.SyncronousActionIsDone(e.ActionId)))
        {
            yield return new WaitForFixedUpdate();
        }

        doAfter();
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

    private class SlotActionWrapper
    {
        public SlotActionWrapper(
            InventorySlotUI inventorySlot,
            Guid actionId)
        {
            InventorySlot = inventorySlot;
            ActionId = actionId;
        }

        public InventorySlotUI InventorySlot { get; }
        public Guid ActionId { get; }
    }
}

