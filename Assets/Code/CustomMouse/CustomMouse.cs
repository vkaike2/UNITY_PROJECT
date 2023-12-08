using System;
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

    public bool IsDragging { get; private set; } = false;
    public TwoStepsUsableItemUI ItemBeingUsed { get; set; }


    private MouseOver _mouseOverItem = null;
    private InventoryDraggableUI _inventoryDraggableUI;

    private void Start()
    {
        _inventoryDraggableUI = GameObject.FindObjectOfType<InventoryDraggableUI>();
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

        if (ItemBeingUsed != null)
        {
            StopInteractionWithTwoStepsUsableItem();
            return;
        }

        MouseInteractable mouseInteractable = GetMouseInteractableUnderMouse();

        if (mouseInteractable != null)
        {
            mouseInteractable.InteractWith(this);
            return;
        }

        TryToDragItem();
    }

    public void OnRightMouseButton(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        UsableItemUI usableItem = RaycastUtils
           .GetComponentsUnderMouseUI<UsableItemUI>(new List<RaycastUtils.Excluding>() { RaycastUtils.Excluding.Children })
           .FirstOrDefault();

        if(usableItem != null && usableItem.CanUseItem()) 
        {
            usableItem.UseItem();
            return;
        }

        StartInteractionWithTwoStepsUsableItem();
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

        MouseOver mouseOverItem = GetMouseOverObjectUnderMouse();
        if (mouseOverItem == null)
        {
            if (_mouseOverItem != null)
            {
                _mouseOverItem.ChangeAnimationOnItemOver(false);
                _mouseOverItem = null;
            }
            return;
        }

        if (_mouseOverItem != null && _mouseOverItem.GetInstanceID() != mouseOverItem.GetInstanceID())
        {
            _mouseOverItem.ChangeAnimationOnItemOver(false);
        }

        mouseOverItem.ChangeAnimationOnItemOver(true);

        _mouseOverItem = mouseOverItem;
    }

    private MouseOver GetMouseOverObjectUnderMouse()
    {
        if (RaycastUtils.HitSomethingUnderMouseUI())
        {
            return RaycastUtils.GetComponentsUnderMouseUI<MouseOver>(new List<RaycastUtils.Excluding>()
                {
                RaycastUtils.Excluding.Children
                })
            .OrderBy(e => e.Priority)
            .FirstOrDefault();
        }
        return RaycastUtils.GetComponentsUnderMouse<MouseOver>(new List<RaycastUtils.Excluding>() { RaycastUtils.Excluding.Parent })
            .OrderBy(e => e.Priority)
            .FirstOrDefault();
    }

    private MouseInteractable GetMouseInteractableUnderMouse()
    {
        return RaycastUtils.GetComponentsUnderMouse<MouseInteractable>(new List<RaycastUtils.Excluding>() { RaycastUtils.Excluding.Parent })
            .OrderBy(e => e.Priority)
            .FirstOrDefault();
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

    private void StartInteractionWithTwoStepsUsableItem()
    {
        TwoStepsUsableItemUI twoStepsUsableItemUI = RaycastUtils
           .GetComponentsUnderMouseUI<TwoStepsUsableItemUI>(new List<RaycastUtils.Excluding>() { RaycastUtils.Excluding.Children })
           .FirstOrDefault();

        if (twoStepsUsableItemUI != null && twoStepsUsableItemUI.CanBeUsed())
        {
            ItemBeingUsed = twoStepsUsableItemUI.GetItemToHand();
            _inventoryDraggableUI.StartUsingItem(ItemBeingUsed);
            return;
        }
    }

    private void StopInteractionWithTwoStepsUsableItem()
    {
        _inventoryDraggableUI.StopUsingItem();

        InventoryItemUI itemUnderMouse = RaycastUtils
            .GetComponentsUnderMouseUI<InventoryItemUI>(new List<RaycastUtils.Excluding>() { RaycastUtils.Excluding.Children })
            .FirstOrDefault();

        if (itemUnderMouse != null && itemUnderMouse.ItemData.Item.RotatedItem != null)
        {
            ItemData itemData = itemUnderMouse.ItemData;
            itemUnderMouse.RemoveFromInventory();

            ItemBeingUsed.UseOnItem(itemData);

            StartDragItem(itemData);

            ItemBeingUsed = null;
            return;
        }

        ItemBeingUsed.ReturnItemToSlot();
        ItemBeingUsed = null;
    }

    public enum DragAction
    {
        Stop, Continue
    }
}
