using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using static CustomMouse;

public class InventoryDraggableUI : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private ItemBeingUsedUI _itemBeingUsedUI;

    private bool _isUsingItem = false;
    private bool _isBeingDragged = false;
    private InventoryItemUI _activePrefab;
    private InventoryUI _inventoryUI;
    private GameManager _gameManager;

    private void Start()
    {
        _inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();

        _itemBeingUsedUI.ActivateImage(_isUsingItem);
    }

    private void Update()
    {
        if (!_isBeingDragged && !_isUsingItem) return;

        this.transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void StartDragItem(ItemData itemData)
    {
        _activePrefab = Instantiate(itemData.Item.PrefabUI, this.transform);
        _activePrefab.transform.position = this.transform.position;
        _activePrefab.ItemData = itemData;
        _activePrefab.IsBeingDragged = true;

        _isBeingDragged = true;
    }

    public DragAction StopDragItem()
    {
        DragAction dragAction;

        bool dropInsideInventory = RaycastUtils.GetComponentsUnderMouseUI<InventoryUI>().Any();
        Debug.Log(dropInsideInventory);
        if (dropInsideInventory)
        {
            dragAction = ManagItemDropInsideInventory();
        }
        else
        {
            DropItemOnTheGround();
            Destroy(_activePrefab.gameObject);
            dragAction = CustomMouse.DragAction.Stop;
        }

        if (dragAction == DragAction.Stop)
        {
            _activePrefab.StopDrag();
            _activePrefab = null;
            _isBeingDragged = false;
        }

        return dragAction;
    }

    public void StartUsingItem(TwoStepsUsableItemUI itemBeingUsed)
    {
        _isUsingItem = true;

        _itemBeingUsedUI.ActivateImage(_isUsingItem, itemBeingUsed.DraggableSprite);
    }

    public void StopUsingItem()
    {
        _isUsingItem = false;
        _itemBeingUsedUI.ActivateImage(_isUsingItem);
    }

    private DragAction ManagItemDropInsideInventory()
    {
        bool itHasBeingAddedToInventory = TryToAddItemToInventory();

        if (itHasBeingAddedToInventory)
        {
            return DragAction.Stop;
        }

        bool itHasBeingAbleToSwap = TryToSwapItensInYourHand();

        return DragAction.Continue;
    }

    private bool TryToAddItemToInventory()
    {
        InventorySlotUI slot = RaycastUtils.GetComponentsUnderMouseUI<InventorySlotUI>().FirstOrDefault();
        if (slot == null) return false;

        bool canFit = _activePrefab.CheckIfCanFit();
        if (!canFit) return false;

        _inventoryUI.AddItem(_activePrefab);

        return true;
    }

    private bool TryToSwapItensInYourHand()
    {
        if (!_activePrefab.CheckIfCanSwap()) return false;

        InventoryItemUI itemToSwap = _activePrefab.GetItemToSwap();
        if (itemToSwap == null) return false;

        ItemData itemDataToSwap = itemToSwap.ItemData;
        itemToSwap.RemoveFromInventory();

        _activePrefab.ItemData.HasBeeingSwaped = true;
        _inventoryUI.AddItem(_activePrefab);
        _activePrefab = null;

        StartDragItem(itemDataToSwap);
        return true;
    }

    private void DropItemOnTheGround()
    {
        _gameManager.Player.PlayerInventory.DropItem(_activePrefab.ItemData);
    }

}
