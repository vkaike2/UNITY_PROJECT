using System;


public class ItemData
{
    public ItemData(Guid id, ScriptableItem item)
    {
        Id = id;
        Item = item;
    }

    public Guid Id { get; set; }
    public ScriptableItem Item { get; set; }
}
