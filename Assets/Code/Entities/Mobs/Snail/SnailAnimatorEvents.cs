using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SnailAnimatorEvents : MonoBehaviour
{

    [SerializeField]
    private OnToggleMovement _onCanMove;
    [field: SerializeField]
    public UnityEvent OnSetInitialBehaviour { get; private set; } = new UnityEvent();


    public void SetInitialBehaviour() => OnSetInitialBehaviour.Invoke();
    public void CanMove() => _onCanMove.Invoke(true);
    public void CanNotMove() => _onCanMove.Invoke(false);
}
