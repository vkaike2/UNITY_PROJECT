using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [Header("EVENTS")]
    [SerializeField]
    private ScriptableItemEvents _itemEvents;

    private InventoryData _inventoryData = null;
    private InventoryData _equipData = null;

    private void Start()
    {
        UIEventManager.instance.OnInventoryChange.AddListener(OnInventoryChange);
    }

    public void LoadInventoryDataFromMemory()
    {
        if (!SaveLoadManager.HasInventoryInfo()) return;
        Debug.Log("Loaded");

        _inventoryData = SaveLoadManager.InventoryData;
        _equipData = SaveLoadManager.EquipData;

        foreach (var itemData in _equipData.Itens)
        {
            if (!itemData.IsEquiped) continue;

            _itemEvents.OnEquipItem.Invoke(itemData.Item);
        }
    }

    public void OnOpenInventoryInput(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        UIEventManager.instance.OnToggleInventoryOpen.Invoke();
    }

    public void DropItem(ItemData itemData)
    {
        ItemDrop itemDrop = Instantiate(itemData.Item.ItemDrop, this.transform);
        itemDrop.transform.parent = null;
        itemDrop.ItemData = itemData;
        itemDrop.DropItem();
    }

    #region CAN EQUIP ITEM
    public bool CanEquipItem(ItemData itemData)
    {
        List<ScriptableItem> equipedItens = _equipData?.Itens
            .Where(e => e.IsEquiped && e.Id != itemData.Id)
            .Select(e => e.Item).ToList();

        equipedItens ??= new List<ScriptableItem>();

        if (!CheckIfReachedItemLimitation(equipedItens, itemData)) return false;

        if (itemData.Item.Type == ScriptableItem.ItemType.Minor) return true;

        return !equipedItens.Any(e => e.Type == ScriptableItem.ItemType.Major && e.Target == itemData.Item.Target);
    }

    private bool CheckIfReachedItemLimitation(List<ScriptableItem> equipedItens, ItemData itemData)
    {
        if (!itemData.Item.Identity.HasLimit) return true;
        int amountOfItem = equipedItens.Count(e => e.Identity.name == itemData.Item.Identity.name);
        return itemData.Item.Identity.Limit > amountOfItem;
    }

    #endregion

    #region CHECK IF CAN ADD ITEM
    /// <summary>
    ///     Returns an List of Coordinate where the item will be alocated
    /// </summary>
    /// <param name="itemLayout"></param>
    /// <returns></returns>
    public List<Vector2> CheckIfCanAddItem(ScriptableItem.ItemLayout itemLayout)
    {
        return itemLayout switch
        {
            ScriptableItem.ItemLayout.OneByOne => CheckIfCanAddItemOneByOne(_inventoryData),
            ScriptableItem.ItemLayout.OneByTwo => CheckIfCanAddItemOneByTwo(_inventoryData),
            ScriptableItem.ItemLayout.TwoByOne => CheckIfCanAddItemTwoByOne(_inventoryData),
            ScriptableItem.ItemLayout.TwoByTwo => CheckIfCanAddItemTwoByTwo(_inventoryData),
            ScriptableItem.ItemLayout.TwoByThree => CheckIfCanAddItemTwoByThree(_inventoryData),
            ScriptableItem.ItemLayout.ThreeByTwo => CheckIfCanAddItemThreeByTwo(_inventoryData),
            _ => null,
        };
    }
    //1969 

    public List<Vector2> CheckIfCanAutoEquip(ScriptableItem.ItemLayout itemLayout, ItemData itemData)
    {
        if (!this.CanEquipItem(itemData)) return null;

        return itemLayout switch
        {
            ScriptableItem.ItemLayout.OneByOne => CheckIfCanAddItemOneByOne(_equipData),
            ScriptableItem.ItemLayout.OneByTwo => CheckIfCanAddItemOneByTwo(_equipData),
            ScriptableItem.ItemLayout.TwoByOne => CheckIfCanAddItemTwoByOne(_equipData),
            ScriptableItem.ItemLayout.TwoByTwo => CheckIfCanAddItemTwoByTwo(_equipData),
            ScriptableItem.ItemLayout.TwoByThree => CheckIfCanAddItemTwoByThree(_equipData),
            ScriptableItem.ItemLayout.ThreeByTwo => CheckIfCanAddItemThreeByTwo(_equipData),
            _ => null,
        };
    }

    private List<Vector2> CheckIfCanAddItemOneByOne(InventoryData inventoryData)
    {
        return CheckIfCanAddItem((Vector2 coordinate) =>
        {
            return new List<Vector2>()
            {
                coordinate
            };
        }, inventoryData);
    }

    private List<Vector2> CheckIfCanAddItemOneByTwo(InventoryData inventoryData)
    {
        return CheckIfCanAddItem((Vector2 coordinate) =>
        {
            return new List<Vector2>()
            {
                coordinate,
                new Vector2(coordinate.x, coordinate.y -1),
            };
        }, inventoryData);

    }

    private List<Vector2> CheckIfCanAddItemTwoByOne(InventoryData inventoryData)
    {
        return CheckIfCanAddItem((Vector2 coordinate) =>
        {
            return new List<Vector2>()
            {
                coordinate,
                new Vector2(coordinate.x+1, coordinate.y),
            };
        }, inventoryData);
    }

    private List<Vector2> CheckIfCanAddItemTwoByTwo(InventoryData inventoryData)
    {
        return CheckIfCanAddItem((Vector2 coordinate) =>
        {
            return new List<Vector2>()
            {
                coordinate,
                new Vector2(coordinate.x +1, coordinate.y),

                new Vector2(coordinate.x, coordinate.y -1),
                new Vector2(coordinate.x +1, coordinate.y -1),
            };
        }, inventoryData);
    }

    private List<Vector2> CheckIfCanAddItemTwoByThree(InventoryData inventoryData)
    {
        return CheckIfCanAddItem((Vector2 coordinate) =>
        {
            return new List<Vector2>()
            {
                coordinate,
                new Vector2(coordinate.x +1, coordinate.y),

                new Vector2(coordinate.x, coordinate.y -1),
                new Vector2(coordinate.x +1, coordinate.y -1),

                new Vector2(coordinate.x, coordinate.y -2),
                new Vector2(coordinate.x +1, coordinate.y -2),
            };
        }, inventoryData);
    }

    private List<Vector2> CheckIfCanAddItemThreeByTwo(InventoryData inventoryData)
    {
        return CheckIfCanAddItem((Vector2 coordinate) =>
        {
            return new List<Vector2>()
            {
                coordinate,
                new Vector2(coordinate.x +1, coordinate.y),
                new Vector2(coordinate.x +2, coordinate.y),

                new Vector2(coordinate.x, coordinate.y -1),
                new Vector2(coordinate.x +1, coordinate.y -1),
                new Vector2(coordinate.x +2, coordinate.y -1),
            };
        }, inventoryData);
    }

    private List<Vector2> CheckIfCanAddItem(Func<Vector2, List<Vector2>> getRequiredCoordinates, InventoryData inventoryData)
    {
        List<InventoryData.Slot> emptySlots = inventoryData.Slots
            .Where(e => !e.HasItem && e.IsAvailable)
            .OrderBy(e => e.Coordinate.NumericOrder())
            .ToList();

        foreach (var slot in emptySlots)
        {
            List<Vector2> requiredCoordinates = getRequiredCoordinates(slot.Coordinate);

            if (emptySlots.Count(e => requiredCoordinates.Contains(e.Coordinate)) == requiredCoordinates.Count)
            {
                return requiredCoordinates;
            }
        }

        return null;
    }
    #endregion

    #region ADD ITEM
    public void AddItem(ItemData itemData, List<Vector2> coordinates)
    {
        List<InventoryData.Slot> itemSlots = _inventoryData.Slots.Where(e => coordinates.Contains(e.Coordinate)).ToList();

        AddItemToSlots(itemSlots, itemData.Id);
        _inventoryData.Itens.Add(itemData);

        UIEventManager.instance.OnInventoryChange.Invoke(_inventoryData, EventSentBy.Player);
    }

    public void EquipItem(ItemData itemData, List<Vector2> coordinates)
    {
        List<InventoryData.Slot> itemSlots = _equipData.Slots.Where(e => coordinates.Contains(e.Coordinate)).ToList();
        AddItemToSlots(itemSlots, itemData.Id);

        _equipData.Itens.Add(itemData);

        EquipSingleItem(itemData);

        UIEventManager.instance.OnInventoryChange.Invoke(_equipData, EventSentBy.Player);
    }

    private void AddItemToSlots(List<InventoryData.Slot> itemSlots, Guid itemId)
    {
        foreach (var slot in itemSlots)
        {
            slot.AddItem(itemId);
        }
    }

    #endregion

    private void OnInventoryChange(InventoryData inventoryData, EventSentBy sentBy)
    {
        if (sentBy == EventSentBy.Player) return;

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
        _inventoryData = inventoryData;
    }

    private void UpdateEquipFromEvent(InventoryData equipData)
    {
        List<Guid> newItemsIds = equipData?.Itens.Select(x => x.Id).ToList();

        bool someEquipmentWasUnequiped = UnequipItem(GetEquipedItensId(), newItemsIds);

        EquipItems(equipData, newItemsIds);

        _equipData = equipData;

        if (someEquipmentWasUnequiped)
        {
            UIEventManager.instance.OnInventoryRemoveEquipment.Invoke();
        }
    }

    private List<Guid> GetEquipedItensId()
    {
        List<Guid> equipedItemId = new List<Guid>();
        if (_equipData != null)
        {
            equipedItemId = _equipData?.Itens.Select(x => x.Id).ToList();
        }
        return equipedItemId;
    }

    private void EquipItems(InventoryData equipData, List<Guid> newItemsIds)
    {
        foreach (var itemId in newItemsIds)
        {
            ItemData itemData = equipData.Itens.Where(e => e.Id == itemId).FirstOrDefault();

            if (!EquipSingleItem(itemData)) continue;

            //if (itemData == null) continue;
            //if (itemData.IsEquiped) continue;
            //if (!itemData.Item.IsEquipable) continue;
            //if (!CanEquipItem(itemData)) continue;

            //_itemEvents.OnEquipItem.Invoke(itemData.Item);

            //itemData.IsEquiped = true;

            //if (itemData.HasBeeingSwaped)
            //{
            //    UIEventManager.instance.OnInventoryRemoveEquipment.Invoke();
            //}

            //itemData.HasBeeingSwaped = false;
        }
    }

    private bool EquipSingleItem(ItemData itemData)
    {
        if (itemData == null) return false;
        if (itemData.IsEquiped) return false;
        if (!itemData.Item.IsEquipable) return false;
        if (!CanEquipItem(itemData)) return false;

        _itemEvents.OnEquipItem.Invoke(itemData.Item);

        itemData.IsEquiped = true;

        if (itemData.HasBeeingSwaped)
        {
            UIEventManager.instance.OnInventoryRemoveEquipment.Invoke();
        }

        itemData.HasBeeingSwaped = false;

        return true;
    }

    private bool UnequipItem(List<Guid> equipedItemId, List<Guid> newItemsIds)
    {
        bool someEquipmentWasUnequiped = false;

        List<Guid> itensToUnEquipIds = equipedItemId.Where(id => !newItemsIds.Contains(id)).ToList();
        foreach (var itemId in itensToUnEquipIds)
        {
            ItemData itemData = _equipData.Itens.Where(e => e.Id == itemId).FirstOrDefault();
            if (itemData == null) continue;
            if (!itemData.Item.IsEquipable) continue;

            someEquipmentWasUnequiped = true;
            _itemEvents.OnUnequipItem.Invoke(itemData.Item);

            itemData.IsEquiped = false;
        }

        if (itensToUnEquipIds.Any())
        {
            _equipData.Itens = _equipData.Itens.Where(e => !itensToUnEquipIds.Contains(e.Id)).ToList();
        }

        return someEquipmentWasUnequiped;
    }

    private void OnDestroy()
    {
        SaveInventoryDataOnMemory();
    }

    private void SaveInventoryDataOnMemory()
    {
        Debug.Log("Saved");
        SaveLoadManager.InventoryData = _inventoryData;
        SaveLoadManager.EquipData = _equipData;
    }
}