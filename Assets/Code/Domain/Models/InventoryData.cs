using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData
{
    public InventoryData(SlotType type)
    {
        Type = type;
    }

    public SlotType Type { get; set; }

    public List<Slot> Slots { get; set; }
    public List<ItemData> Itens { get; set; }

    public class Slot
    {
        public Vector2 Coordinate { get; set; }
        public bool HasItem { get; set; }
        public bool IsAvailable { get; set; } = true;
        public Guid? ItemId { get; set; }
    
        public void AddItem(Guid itemId)
        {
            ItemId = itemId;
            HasItem = true;
        }

        public void RemoveItem()
        {
            ItemId = null;
            HasItem = false;
        }
    }
}
