using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryItemUI : MonoBehaviour
{
    public bool IsBeingDragged { get; set; }

    public ItemData ItemData { get; set; }

    private List<InventoryItemSlotUI> _slots;
    private List<InventorySlotUI> _tempInventorySlots = new List<InventorySlotUI>();
    private InventoryUI _inventory = null;

    private void Awake()
    {
        _slots = GetComponentsInChildren<InventoryItemSlotUI>().ToList();
    }

    private void Start()
    {
        _inventory = GameObject.FindObjectOfType<InventoryUI>();
    }

    private void FixedUpdate()
    {
        ManageDrag();
    }

    public bool CheckIfCanFit()
    {
        List<InventorySlotUI> slotsUnderItem = GetEveryInventorySlotUnderItem();
        slotsUnderItem = slotsUnderItem.Where(e => !e.HasItem).ToList();

        return slotsUnderItem.Count == _slots.Count;
    }

    public bool CheckIfCanSwap()
    {
        List<InventorySlotUI> slotsUnderItem = GetEveryInventorySlotUnderItem();

        if (slotsUnderItem.Count != _slots.Count) return false;
        if (slotsUnderItem.Where(e => e.HasItem).Select(e => e.ItemUI.ItemData.Id).Distinct().Count() > 1) return false;

        return true;
    }

    public List<InventorySlotUI> AddToInventory(Transform itemParent, InventoryUI inventory)
    {
        List<InventorySlotUI> inventorySlotsUnderItem = GetEveryInventorySlotUnderItem();

        this.transform.position = CalculateMiddlePosition(inventorySlotsUnderItem.Select(e => (Vector2) e.transform.position).ToList());
        this.transform.SetParent(itemParent);

        return inventorySlotsUnderItem;
    }

    public void RemoveFromInventory()
    {
        _inventory.RemoveItem(ItemData.Id);
        Destroy(this.gameObject);
    }

    public void StopDrag()
    {
        IsBeingDragged = false;

        foreach (var slot in GetEveryInventorySlotUnderItem())
        {
            slot.ChangeAnimationOnItemOver(false);
        }
    }

    public List<InventorySlotUI> GetEveryInventorySlotUnderItem()
    {
        List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();
        foreach (InventoryItemSlotUI slot in _slots)
        {
            InventorySlotUI inventorySlot = RaycastUtils
                .GetComponentsUnderPositionUI<InventorySlotUI>(slot.transform.position, new List<RaycastUtils.Excluding>() { RaycastUtils.Excluding.Children })
                .FirstOrDefault();

            if (inventorySlot == null) continue;
            inventorySlots.Add(inventorySlot);
        }

        return inventorySlots;
    }

    public InventoryItemUI GetItemToSwap()
    {
        List<InventorySlotUI> slotsUnderItem = GetEveryInventorySlotUnderItem();
        return slotsUnderItem.Where(e => e.HasItem).Select(e => e.ItemUI).FirstOrDefault();
    }

    private void ManageDrag()
    {
        if (!IsBeingDragged) return;

        List<InventorySlotUI> inventorySlotsUnderItem = GetEveryInventorySlotUnderItem();

        foreach (InventorySlotUI slot in _tempInventorySlots.Where(e => inventorySlotsUnderItem.FirstOrDefault(s => s.GetInstanceID() == e.GetInstanceID()) == null))
        {
            slot.ChangeAnimationOnItemOver(false);
        }

        _tempInventorySlots.Clear();
        _tempInventorySlots.AddRange(inventorySlotsUnderItem);

        foreach (InventorySlotUI slot in _tempInventorySlots)
        {
            slot.ChangeAnimationOnItemOver(true);
        }
    }


    private Vector2 CalculateMiddlePosition(List<Vector2> positions)
    {
        Vector2 sum = Vector2.zero;
        for (int i = 0; i < positions.Count; i++)
        {
            sum += positions[i];
        }
        Vector2 middlePosition = sum / positions.Count;
        return middlePosition;
    }
}
