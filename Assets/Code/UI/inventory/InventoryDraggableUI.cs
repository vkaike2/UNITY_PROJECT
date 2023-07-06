using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CustomMouse;

public class InventoryDraggableUI : MonoBehaviour
{
    private bool _isBeingDragged = false;
    private InventoryItemUI _activePrefab;
    private InventoryUI _inventoryUI;
    private GameManager _gameManager;

    private void Start()
    {
        _inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (!_isBeingDragged) return;
        this.transform.position = Input.mousePosition;
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

        if(dragAction == DragAction.Stop)
        {
            _activePrefab.StopDrag();
            _activePrefab = null;
            _isBeingDragged = false;
        }

        return dragAction;
    }

    private DragAction ManagItemDropInsideInventory()
    {
        bool isHasBeingAddedToInventory = TryToAddItemToInventory();

        if (isHasBeingAddedToInventory)
        {
            return DragAction.Stop;
        }

        TryToSwapItensInYourHand();

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

    private void TryToSwapItensInYourHand()
    {
        if (!_activePrefab.CheckIfCanSwap()) return;

        InventoryItemUI itemToSwap = _activePrefab.GetItemToSwap();
        ItemData itemDataToSwap = itemToSwap.ItemData;
        itemToSwap.RemoveFromInventory();

        _inventoryUI.AddItem(_activePrefab);
        _activePrefab = null;

        StartDragItem(itemDataToSwap);
    }

    private void DropItemOnTheGround()
    {
        _gameManager.Player.PlayerInventory.DropItem(_activePrefab.ItemData);
    }

}
