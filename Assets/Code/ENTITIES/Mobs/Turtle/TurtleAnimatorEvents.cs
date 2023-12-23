using UnityEngine;
using UnityEngine.Events;

public class TurtleAnimatorEvents : MonoBehaviour
{
    [field: SerializeField]
    public UnityEvent OnSetInitialBehaviour { get; private set; } = new UnityEvent();

    public void SetInitialBehaviour() => OnSetInitialBehaviour.Invoke();
}