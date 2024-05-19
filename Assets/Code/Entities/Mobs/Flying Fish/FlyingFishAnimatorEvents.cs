using UnityEngine;
using UnityEngine.Events;

public class FlyingFishAnimatorEvents : MonoBehaviour
{
    [field: SerializeField]
    public UnityEvent OnSetInitialBehaviour { get; private set; } = new UnityEvent();
    [field: SerializeField]
    public UnityEvent OnStartMoving {get; private set;} = new UnityEvent();

    public void SetInitialBehaviour() => OnSetInitialBehaviour.Invoke();

    public void StartMoving() => OnStartMoving.Invoke();
}