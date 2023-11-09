using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class ScriptableItem : ScriptableObject
{
    [field: Header("IDENTITY")]
    [field: SerializeField]
    public ScriptableItemIdentity Identity { get; private set; }

    [field: Header("GENERAL")]
    //[field: SerializeField]
    //public string Name { get; private set; }
    //[field: TextArea]
    //[field: SerializeField]
    //public string Description { get; private set; }    
    [field: SerializeField]
    public ScriptableItem RotatedItem { get; private set; }
    //[field: SerializeField]
    //public bool HasLimit { get; private set; } = false;
    //[field: SerializeField]
    //public int Limit { get; private set; }


    [field: Header("INVENTORY INFO")]
    [field:Tooltip("Only applicable for two steps usage like the Rotational Jam")]
    [field: SerializeField]
    public Sprite SpriteWhileUsing { get; private set; }
    [field: SerializeField]
    public InventoryItemUI PrefabUI { get; private set; }
    [field: SerializeField]
    public ItemLayout InventoryItemLayout { get; private set; }


    [field: Header("EQUIPABLE INFO")]
    [field: SerializeField]
    public bool IsEquipable { get; set; }
    [field: SerializeField]
    public ItemType Type { get; set; }
    [field: SerializeField]
    public ItemTarget Target { get; set; }


    [field: Header("DROP")]
    [field: SerializeField]
    public ItemDrop ItemDrop { get; private set; }

    public enum ItemLayout
    {
        OneByOne,
        OneByTwo,
        TwoByOne,
        TwoByTwo,
        TwoByThree,
        ThreeByTwo
    }

    public enum ItemType
    {
        Major,
        Minor
    }

    public enum ItemTarget
    {
        Poop,
        Fart
    }
}
