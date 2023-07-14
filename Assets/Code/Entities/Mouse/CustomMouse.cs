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

    private MouseInteractable _mouseInteractable = null;

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

        if (IsDragging)
        {
            StopDragItem();
            return;
        }

        MouseInteractable mouseInteractable = RaycastUtils.GetComponentsUnderMouse<MouseInteractable>()
            .OrderBy(e => e.Priority)
            .FirstOrDefault();
        if (mouseInteractable != null)
        {
            mouseInteractable.InteractWith(this);
            return;
        }


        TryToDragItem();
    }

    public void OnRightMouseButton(InputAction.CallbackContext context)
    {
    }
    #endregion

    public void StartDragItem(ItemData itemData)
    {
        _inventoryDraggableUI.StartDragItem(itemData);
        IsDragging = true;
    }

    private void ManageMouseOver()
    {
        if (IsDragging) return;

        MouseInteractable mouseInteractable = RaycastUtils.GetComponentsUnderMouse<MouseInteractable>()
            .OrderBy(e => e.Priority)
            .FirstOrDefault();

        if (mouseInteractable == null)
        {
            if (_mouseInteractable != null)
            {
                _mouseInteractable.ChangeAnimationOnItemOver(false);
                _mouseInteractable = null;
            }
            return;
        }

        if (_mouseInteractable != null && _mouseInteractable.GetInstanceID() != mouseInteractable.GetInstanceID())
        {
            _mouseInteractable.ChangeAnimationOnItemOver(false);
        }

        mouseInteractable.ChangeAnimationOnItemOver(true);

        _mouseInteractable = mouseInteractable;
    }

    private void GameObjectFollowMouse() => transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void StopDragItem()
    {
        DragAction action = _inventoryDraggableUI.StopDragItem();
        IsDragging = action != DragAction.Stop;
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

    public enum DragAction
    {
        Stop, Continue
    }
}
