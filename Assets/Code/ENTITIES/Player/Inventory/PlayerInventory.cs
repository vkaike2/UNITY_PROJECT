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
        LoggerUtils.Log("Loaded");

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

    public bool CanEquipItem(ItemData itemData)
    {
        List<ScriptableItem> equipedItens = _equipData?.Itens
            .Where(e => e.IsEquiped && e.Id != itemData.Id)
            .Select(e => e.Item).ToList();

        equipedItens ??= new List<ScriptableItem>();

        if (itemData.Item.Type == ScriptableItem.ItemType.Minor) return true;
        return !equipedItens.Any(e => e.Type == ScriptableItem.ItemType.Major && e.Affect == itemData.Item.Affect);
    }

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
            ScriptableItem.ItemLayout.OneByOne => CheckIfCanAddItemOneByOne(),
            ScriptableItem.ItemLayout.OneByTwo => CheckifCanAddItemOneByTwo(),
            ScriptableItem.ItemLayout.TwoByOne => CheckIfCanAddItemTwoByOne(),
            ScriptableItem.ItemLayout.TwoByTwo => CheckifCanAddItemTwoByTwo(),
            ScriptableItem.ItemLayout.TwoByThree => CheckifCanAddItemTwoByThree(),
            ScriptableItem.ItemLayout.ThreeByTwo => CheckIfCanAddItemThreeByTwo(),
            _ => null,
        };
    }

    private List<Vector2> CheckIfCanAddItemOneByOne()
    {
        return CheckIfCanAdditem((Vector2 coordinate) =>
        {
            return new List<Vector2>()
            {
                coordinate
            };
        });
    }

    private List<Vector2> CheckifCanAddItemOneByTwo()
    {
        return CheckIfCanAdditem((Vector2 coordinate) =>
        {
            return new List<Vector2>()
            {
                coordinate,
                new Vector2(coordinate.x, coordinate.y -1),
            };
        });

    }

    private List<Vector2> CheckIfCanAddItemTwoByOne()
    {
        return CheckIfCanAdditem((Vector2 coordinate) =>
        {
            return new List<Vector2>()
            {
                coordinate,
                new Vector2(coordinate.x+1, coordinate.y),
            };
        });
    }

    private List<Vector2> CheckifCanAddItemTwoByTwo()
    {
        return CheckIfCanAdditem((Vector2 coordinate) =>
        {
            return new List<Vector2>()
            {
                coordinate,
                new Vector2(coordinate.x +1, coordinate.y),

                new Vector2(coordinate.x, coordinate.y -1),
                new Vector2(coordinate.x +1, coordinate.y -1),
            };
        });
    }

    private List<Vector2> CheckifCanAddItemTwoByThree()
    {
        return CheckIfCanAdditem((Vector2 coordinate) =>
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
        });
    }

    private List<Vector2> CheckIfCanAddItemThreeByTwo()
    {
        return CheckIfCanAdditem((Vector2 coordinate) =>
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
        });
    }

    private List<Vector2> CheckIfCanAdditem(Func<Vector2, List<Vector2>> getRequiredCoordinates)
    {
        List<InventoryData.Slot> emptySlots = _inventoryData.Slots.Where(e => !e.HasItem).ToList();

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

        EquipItem(equipData, newItemsIds);

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

    private void EquipItem(InventoryData equipData, List<Guid> newItemsIds)
    {
        foreach (var itemId in newItemsIds)
        {
            ItemData itemData = equipData.Itens.Where(e => e.Id == itemId).FirstOrDefault();

            if (itemData == null) continue;
            if (itemData.IsEquiped) continue;
            if (!itemData.Item.IsEquipable) continue;
            if (!CanEquipItem(itemData)) continue;

            _itemEvents.OnEquipItem.Invoke(itemData.Item);

            itemData.IsEquiped = true;
           
            if (itemData.HasBeeingSwaped)
            {
                UIEventManager.instance.OnInventoryRemoveEquipment.Invoke();
            }

            itemData.HasBeeingSwaped = false;
        }
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
        LoggerUtils.Log("Saved");
        SaveLoadManager.InventoryData = _inventoryData;
        SaveLoadManager.EquipData = _equipData;
    }
}