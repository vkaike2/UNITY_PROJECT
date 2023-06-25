using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomMouse : MonoBehaviour
{
    public ScriptableItem item;

    
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
                StartDragItem();
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

    private void StartDragItem()
    {
        _inventoryDraggableUI.StartDragItem(item);
        _isDragging = true;
    }

    private void StopDragItem()
    {
        _inventoryDraggableUI.StopDragItem();
        _isDragging = false;
    }

    private enum MouseButton
    {
        Left, Right
    }
}
