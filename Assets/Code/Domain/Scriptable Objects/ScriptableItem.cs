using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class ScriptableItem : ScriptableObject
{
    [Header("GENERAL")]
    [SerializeField]
    private string _name;
    [TextArea]
    [SerializeField]
    private string _description;

    public string Name => _name;
    public string Description => _description;

    [Header("INVENTORY INFO")]
    [SerializeField]
    private InventoryItemUI _prefabUI;
    [SerializeField]
    private ItemLayout _itemLayout;

    public InventoryItemUI PrefabUI => _prefabUI;
    public ItemLayout InventoryItemLayout => _itemLayout;

    [Header("DROP")]
    [SerializeField]
    private ItemDrop _itemDrop;

    public ItemDrop ItemDrop => _itemDrop;

    public enum ItemLayout
    {
        OneByOne,
        OneByTwo,
        TwoByOne,
        TwoByTwo,
        TwoByThree,
        ThreeByTwo
    }
}
