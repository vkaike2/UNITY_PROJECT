using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static InventoryData;

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
    public bool CheckIfCanAddItem(ScriptableItem.ItemLayout itemLayout)
    {
        return itemLayout switch
        {
            ScriptableItem.ItemLayout.OneByOne => CheckIfCanAddItemOneByOne(),
            ScriptableItem.ItemLayout.OneByTwo => CheckifCanAddItemOneByTwo(),
            ScriptableItem.ItemLayout.TwoByTwo => CheckifCanAddItemTwoByTwo(),
            ScriptableItem.ItemLayout.TwoByThree => CheckifCanAddItemTwoByThree(),
            _ => false,
        };
    }

    private bool CheckIfCanAddItemOneByOne()
    {
        return _inventoryData.Slots.Any(e => !e.HasItem);
    }

    private bool CheckifCanAddItemOneByTwo()
    {
        return false;
    }

    private bool CheckifCanAddItemTwoByTwo()
    {
        return false;
    }

    private bool CheckifCanAddItemTwoByThree()
    {
        return false;
    }
    #endregion

    #region ADD ITEM
    public void AddItem(ItemData itemData)
    {
        switch (itemData.Item.InventoryItemLayout)
        {
            case ScriptableItem.ItemLayout.OneByOne:
                AddItemOneByOne(itemData);
                break;
            default: 
                break;
        }

        _uiEventManager.OnInventoryChange.Invoke(_inventoryData, EventSentBy.Player);
    }

    private void AddItemOneByOne(ItemData itemData)
    {
        Slot emtySlot = _inventoryData.Slots.FirstOrDefault(e => !e.HasItem);
        emtySlot.AddItem(itemData.Id);

        _inventoryData.Itens.Add(itemData);
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
        if (_inventoryData == null)
        {
            _inventoryData = inventoryData;
            return;
        }
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