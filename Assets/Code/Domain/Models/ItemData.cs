using System;

public class ItemData
{
    public ItemData(Guid id, ScriptableItem item)
    {
        Id = id;
        Item = item;
    }

    public Guid Id { get; private set; }
    public ScriptableItem Item { get; set; }

    public bool IsEquiped { get; set; } = false;

    // someone tried to swap an item with this one, requiring it to update the UI
    public bool HasBeeingSwaped { get; set; } = false;
}
