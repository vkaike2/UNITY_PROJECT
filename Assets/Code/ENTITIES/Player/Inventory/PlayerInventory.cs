using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    private InventoryData _inventoryData = null;
    private InventoryData _equipData = null;

    private UIEventManager _uiEventManager;

    private void Start()
    {
        _uiEventManager = GameObject.FindObjectOfType<UIEventManager>();
        _uiEventManager.OnInventoryChange.AddListener(OnInventoryChange);
    }

    public void OnOpenInventoryInput(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        _uiEventManager.OnToggleInventoryOpen.Invoke();
    }

    public void DropItem(ItemData itemData)
    {
        ItemDrop itemDrop = Instantiate(itemData.Item.ItemDrop, this.transform);
        itemDrop.transform.parent = null;
        itemDrop.ItemData = itemData;
        itemDrop.DropItem();
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
            ScriptableItem.ItemLayout.TwoByTwo => CheckifCanAddItemTwoByTwo(),
            ScriptableItem.ItemLayout.TwoByThree => CheckifCanAddItemTwoByThree(),
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

        _uiEventManager.OnInventoryChange.Invoke(_inventoryData, EventSentBy.Player);
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

        if (inventoryData.Type == InventoryData.SlotType.Inventory)
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
        if (_inventoryData == null)
        {
            _equipData = equipData;
            return;
        }
    }
}