using UnityEngine;


public abstract class MouseOver : MonoBehaviour
{
    public int Priority => _priority;

    protected int _priority = 10;

    public abstract void ChangeAnimationOnItemOver(bool isMouseOver);
}
