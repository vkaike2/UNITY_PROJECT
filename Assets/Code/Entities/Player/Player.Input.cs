using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour
{
    public InputModel<Vector2> MoveInput { get; private set; } = new InputModel<Vector2>();
    public InputModel<bool> JumpInput { get; private set; } = new InputModel<bool>();
    public InputModel<bool> FartInput { get; private set; } = new InputModel<bool>();
    public InputModel<bool> DownPlatformInput { get; private set; } = new InputModel<bool>();
    public InputModel<bool> PoopInput { get; private set; } = new InputModel<bool>();

    public void OnDownInput(InputAction.CallbackContext context)
    {
        if (_isFrozen) return;

        InitializeBooleanInput(DownPlatformInput, context);
    }

    public void OnRightMouseButton(InputAction.CallbackContext context)
    {
        if (_isFrozen) return;

        InitializeBooleanInput(PoopInput, context);
    }

    public void OnLeftMouseButton(InputAction.CallbackContext context)
    {
        if (_isFrozen) return;

        InitializeBooleanInput(FartInput, context);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (_isFrozen) return;

        if (context.phase == InputActionPhase.Performed)
        {
            MoveInput.Value = context.ReadValue<Vector2>();
            MoveInput.Performed.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            MoveInput.Value = Vector2.zero;
            MoveInput.Canceled.Invoke();
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        InitializeBooleanInput(JumpInput, context);
    }

    public void OnInteractWithToiletInput(InputAction.CallbackContext context)
    {
        if (!_mapManager.Toilet.CanInteractWithPlayer) return;
        if (context.phase != InputActionPhase.Performed) return;

        Toilet toilet = _mapManager.Toilet;
        toilet.InteractiWithPlayer(this);
    }

    private void InitializeBooleanInput(InputModel<bool> input, InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            input.Value = true;
            input.Performed.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            input.Value = false;
            input.Canceled.Invoke();
        }
    }
}
