using UnityEngine;


public abstract class UsableItemUI : MonoBehaviour
{
    [SerializeField]
    protected ScriptableItemEvents _itemEvents;

    protected GameManager _gameManager;
    protected InventoryItemUI _inventoryItemUI;

    private void Awake()
    {
        _inventoryItemUI = GetComponent<InventoryItemUI>();
    }

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    public abstract bool CanUseItem();

    public abstract void UseItem();
}
