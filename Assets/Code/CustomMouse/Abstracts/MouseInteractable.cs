using UnityEngine;

public abstract class MouseInteractable : MouseOver
{
    /// <summary>
    ///     Interact with Left mouse button 
    /// </summary>
    /// <param name="mouse"></param>
    public abstract void InteractWith(CustomMouse mouse);
}
