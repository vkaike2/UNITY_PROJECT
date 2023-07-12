using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomMouse : MonoBehaviour
{
    public ScriptableItem firstTest;
    public ScriptableItem secondTest;
    public ScriptableItem thirdTest;
    public ScriptableItem fourthTest;

    private InventoryDraggableUI _inventoryDraggableUI;
    public bool IsDragging { get; private set; } = false;

    private ItemDrop _tempItemDrop = null;

    private GameManager _gameManager;

    private void Start()
    {
        _inventoryDraggableUI = GameObject.FindObjectOfType<InventoryDraggableUI>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        GameObjectFollowMouse();
    }

    private void FixedUpdate()
    {
        ManageMouseOver();
    }

    #region TEST INPUTS
    public void OnInputFirstTest(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                break;
            case InputActionPhase.Performed:
                StartDragItem(new ItemData(Guid.NewGuid(), firstTest));
                break;
            case InputActionPhase.Canceled:
                break;
        }
    }
    public void OnInputSecondTest(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                break;
            case InputActionPhase.Performed:
                StartDragItem(new ItemData(Guid.NewGuid(), secondTest));
                break;
            case InputActionPhase.Canceled:
                break;
        }
    }
    public void OnInputThirdTest(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                break;
            case InputActionPhase.Performed:
                StartDragItem(new ItemData(Guid.NewGuid(), thirdTest));
                break;
            case InputActionPhase.Canceled:
                break;
        }
    }
    public void OnInputFourthTest(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                break;
            case InputActionPhase.Performed:
                StartDragItem(new ItemData(Guid.NewGuid(), fourthTest));
                break;
            case InputActionPhase.Canceled:
                break;
        }
    }
    #endregion

    #region INPUT EVENTS
    public void OnLeftMouseButton(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        ItemDrop itemUnderMouse = RaycastUtils.GetComponentsUnderMouse<ItemDrop>().FirstOrDefault();
        if (itemUnderMouse != null)
        {
            TryToPickupItem(itemUnderMouse);
            return;
        }

        if (IsDragging)
        {
            StopDragItem();
        }
        else
        {
            TryToDragItem();
        }
    }

    public void OnRightMouseButton(InputAction.CallbackContext context)
    {
    }
    #endregion

    #region MOUSE OVER
    private void ManageMouseOver()
    {
        if (IsDragging) return;

        ItemDrop itemUnderMouse = RaycastUtils.GetComponentsUnderMouse<ItemDrop>().FirstOrDefault();

        if (itemUnderMouse == null)
        {
            if (_tempItemDrop != null)
            {
                _tempItemDrop.ChangeAnimationOnItemOver(false);
                _tempItemDrop = null;
            }
            return;
        }

        if (_tempItemDrop != null && _tempItemDrop.GetInstanceID() != itemUnderMouse.GetInstanceID())
        {
            _tempItemDrop.ChangeAnimationOnItemOver(false);
        }

        itemUnderMouse.ChangeAnimationOnItemOver(true);

        _tempItemDrop = itemUnderMouse;
    }
    #endregion

    private void GameObjectFollowMouse() => transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void StartDragItem(ItemData itemData)
    {
        _inventoryDraggableUI.StartDragItem(itemData);
        IsDragging = true;
    }

    private void StopDragItem()
    {
        DragAction action = _inventoryDraggableUI.StopDragItem();
        IsDragging = action != DragAction.Stop;
    }

    private void TryToPickupItem(ItemDrop item)
    {
        // Add Item to your hand
        if (_gameManager.InventoryIsOpen)
        {
            StartDragItem(item.ItemData);
            Destroy(item.gameObject);
            return;
        }

        List<Vector2> itemCoordinates = _gameManager.PlayerInventory.CheckIfCanAddItem(item.ItemData.Item.InventoryItemLayout);

        if (itemCoordinates != null)
        {
            _gameManager.PlayerInventory.AddItem(item.ItemData, itemCoordinates);
            Destroy(item.gameObject);
        }
        else
        {
            item.DropItem();
        }
    }

    private void TryToDragItem()
    {
        InventoryItemUI itemUnderMouse = RaycastUtils
            .GetComponentsUnderMouseUI<InventoryItemUI>(new List<RaycastUtils.Excluding>() { RaycastUtils.Excluding.Children })
            .FirstOrDefault();

        if (itemUnderMouse == null) return;

        ItemData itemData = itemUnderMouse.ItemData;

        itemUnderMouse.RemoveFromInventory();

        StartDragItem(itemData);
    }

    private enum MouseButton
    {
        Left, Right
    }

    public enum DragAction
    {
        Stop, Continue
    }
}
