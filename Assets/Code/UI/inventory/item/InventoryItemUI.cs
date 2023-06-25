using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryItemUI : MonoBehaviour
{
    public bool IsBeingDragged { get; set; }

    private List<InventoryItemSlotUI> _slots;
    private ScriptableItem _item = null;

    private List<InventorySlotUI> _tempInventorySlots = new List<InventorySlotUI>();

    private void Awake()
    {
        _slots = GetComponentsInChildren<InventoryItemSlotUI>().ToList();
    }

    private void FixedUpdate()
    {
        ManageDrag();
    }

    public void SetItem(ScriptableItem item)
    {
        _item = item;
    }

    public bool CheckIfCanFit()
    {
        var slotsUnderItem = GetEveryInventorySlotUnderItem();
        return slotsUnderItem.Count == _slots.Count;
    }

    public void SetPositionInsideInventory(Transform itemParent)
    {
        List<InventorySlotUI> inventorySlotsUnderItem = GetEveryInventorySlotUnderItem();

        this.transform.position = CalculateMiddlePosition(inventorySlotsUnderItem.Select(e => (Vector2) e.transform.position).ToList());
        this.transform.SetParent(itemParent);

        foreach (var slot in inventorySlotsUnderItem)
        {
            slot.AddItem();
        }
    }

    public void StopDrag()
    {
        IsBeingDragged = false;

        foreach (var slot in GetEveryInventorySlotUnderItem())
        {
            slot.ChangeAnimationOnItemOver(false);
        }
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

    private List<InventorySlotUI> GetEveryInventorySlotUnderItem()
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
