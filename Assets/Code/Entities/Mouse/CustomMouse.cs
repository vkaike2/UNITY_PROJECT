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

    private bool CheckIfShouldSendClickToPlayer(MouseButton button)
    {
        bool mouseIsNotOverUI = !RaycastUtils.HitSomethingUnderMouseUI();
        return button switch
        {
            MouseButton.Left => mouseIsNotOverUI && !_isDragging,
            MouseButton.Right => mouseIsNotOverUI && !_isDragging,
            _ => false,
        };
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
