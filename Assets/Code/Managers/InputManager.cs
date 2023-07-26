using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private CustomMouse _customMouse;
    private GameManager _gameManager;

    private void Start()
    {
        _customMouse = GameObject.FindObjectOfType<CustomMouse>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void OnLeftMouseButton(InputAction.CallbackContext context)
    {
        switch (ValidateMouseInput(MouseButton.Left))
        {
            case SendMouseTo.Player:
                if (_gameManager.Player == null) return;
                _gameManager.Player.OnLeftMouseButton(context);
                break;
            case SendMouseTo.CustomMouse:
                _customMouse.OnLeftMouseButton(context);
                break;
        }
    }

    public void OnRightMouseButton(InputAction.CallbackContext context)
    {
        switch (ValidateMouseInput(MouseButton.Left))
        {
            case SendMouseTo.Player:
                if (_gameManager.Player == null) return;
                _gameManager.Player.OnRightMouseButton(context);
                break;
            case SendMouseTo.CustomMouse:
                _customMouse.OnRightMouseButton(context);
                break;
        }
    }

    public void OnDownInput(InputAction.CallbackContext context)
    {
        if (_gameManager.Player == null) return;

        _gameManager.Player.OnDownInput(context);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (_gameManager.Player == null) return;

        _gameManager.Player.OnMoveInput(context);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (_gameManager.Player == null) return;

        _gameManager.Player.OnJumpInput(context);
    }

    public void OnToggleInventoryInput(InputAction.CallbackContext context)
    {
        _gameManager.PlayerInventory.OnOpenInventoryInput(context);
    }

    public void OnPauseInput(InputAction.CallbackContext context)
    {
        _gameManager.OnPauseGameInput(context);
        //_gameManager.PlayerInventory.OnOpenInventoryInput(context);
    }


    public void OnInteractEInput(InputAction.CallbackContext context)
    {
        if (_gameManager.Player == null) return;

        _gameManager.Player.OnInteractWithToilet(context);
    }

    private SendMouseTo ValidateMouseInput(MouseButton button)
    {
        bool mouseIsNotOverUI = RaycastUtils.HitSomethingUnderMouseUI();

        if (mouseIsNotOverUI) return SendMouseTo.CustomMouse;
        if (_customMouse.IsDragging) return SendMouseTo.CustomMouse;

        if (button == MouseButton.Left)
        {
            if (RaycastUtils.GetComponentsUnderMouse<MouseInteractable>().FirstOrDefault() != null) return SendMouseTo.CustomMouse;
        }

        return SendMouseTo.Player;
    }

    private enum MouseButton
    {
        Left, Right
    }

    private enum SendMouseTo
    {
        Player,
        CustomMouse
    }
}
