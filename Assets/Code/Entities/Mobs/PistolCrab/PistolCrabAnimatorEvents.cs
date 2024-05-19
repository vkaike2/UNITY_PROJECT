using System;
using UnityEngine;
using UnityEngine.Events;

public class PistolCrabAnimatorEvents : MonoBehaviour
{
    [field: SerializeField]
    public UnityEvent OnSetInitialBehaviour { get; private set; } = new UnityEvent();
    [field: SerializeField]
    public OnPistolCrabShootAnimatorEvent OnPistolCrabShootAnimator { get; private set; } = new OnPistolCrabShootAnimatorEvent();

    public void SetInitialBehaviour() => OnSetInitialBehaviour.Invoke();

    public void ShootPistol(Direction direction) => OnPistolCrabShootAnimator.Invoke(direction);

    public enum Direction
    {
        Left,
        Right
    }

    [Serializable]
    public class OnPistolCrabShootAnimatorEvent : UnityEvent<Direction> { }
}