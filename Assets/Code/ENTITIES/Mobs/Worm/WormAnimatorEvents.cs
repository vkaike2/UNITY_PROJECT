using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WormAnimatorEvents : MonoBehaviour
{
    [SerializeField]
    private OnToggleMovement _onCanMove;
    [SerializeField]
    private UnityEvent _onSetInitialBehaviour;

    public void SetInitialBehaviour() => _onSetInitialBehaviour.Invoke();
    public void CanMove() => _onCanMove.Invoke(true);
    public void CanNotMove() => _onCanMove.Invoke(false);

    [Serializable]
    public class OnToggleMovement : UnityEvent<bool> { }
}
