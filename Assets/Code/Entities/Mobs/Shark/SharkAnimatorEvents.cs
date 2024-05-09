using UnityEngine;
using UnityEngine.Events;

public class SharkAnimatorEvents : MonoBehaviour
{
    [field: SerializeField]
    public UnityEvent OnSetInitialBehaviour { get; private set; } = new UnityEvent();

    public void SetInitialBehaviour() => OnSetInitialBehaviour.Invoke();
}