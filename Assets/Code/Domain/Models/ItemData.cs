using System;
using UnityEngine;

public class ItemData
{
    public ItemData(Guid id, ScriptableItem item)
    {
        Id = id;
        Item = item;
    }

    public Vector2 CurrentPosition { get; set; }
    public Guid Id { get; set; }
    public ScriptableItem Item { get; set; }
}
