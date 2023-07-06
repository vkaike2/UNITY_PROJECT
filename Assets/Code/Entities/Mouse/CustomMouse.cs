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
    private bool _isDragging = false;

    private ItemDrop _tempItemDrop = null;

    //Game Manager
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
        if (CheckIfShouldSendClickToPlayer(MouseButton.Left))
        {
            _gameManager.Player.OnLeftMouseButton(context);
            return;
        }

        if (context.phase != InputActionPhase.Performed) return;

        ItemDrop itemUnderMouse = RaycastUtils.GetComponentsUnderMouse<ItemDrop>().FirstOrDefault();
        if (itemUnderMouse != null)
        {
            TryToPickupItem(itemUnderMouse);
            return;
        }

        if (_isDragging)
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
        if (CheckIfShouldSendClickToPlayer(MouseButton.Right))
        {
            _gameManager.Player.OnRightMouseButton(context);
            return;
        }
    }
    #endregion

    #region MOUSE OVER
    private void ManageMouseOver()
    {
        if (_isDragging) return;

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

    //TODO: MOVE TO MOUSE MANAGER
    private bool CheckIfShouldSendClickToPlayer(MouseButton button)
    {
        bool mouseIsNotOverUI = RaycastUtils.HitSomethingUnderMouseUI();

        // will return true if need to send to player
        if (button == MouseButton.Left)
        {
            if (mouseIsNotOverUI) return false;
            if (_isDragging) return false;

            if (RaycastUtils.GetComponentsUnderMouse<ItemDrop>().FirstOrDefault() != null) return false;

            return true;
        }
        else
        {
            if (mouseIsNotOverUI) return false;
            if (_isDragging) return false;

            return true;
        }
    }

    private void GameObjectFollowMouse() => transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void StartDragItem(ItemData itemData)
    {
        _inventoryDraggableUI.StartDragItem(itemData);
        _isDragging = true;
    }

    private void StopDragItem()
    {
        DragAction action = _inventoryDraggableUI.StopDragItem();
        _isDragging = action != DragAction.Stop;
    }

    private void TryToPickupItem(ItemDrop item)
    {
        if (_gameManager.PlayerInventory.CheckIfCanAddItem(item.ItemData.Item.InventoryItemLayout))
        {
            _gameManager.PlayerInventory.AddItem(item.ItemData);
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
