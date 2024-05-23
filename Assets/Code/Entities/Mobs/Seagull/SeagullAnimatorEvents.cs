using UnityEngine;
using UnityEngine.Events;

public class SeagullAnimatorEvents : MonoBehaviour
{
    [field: SerializeField]
    public UnityEvent OnSetInitialBehaviour { get; private set; } = new UnityEvent();
    [field: SerializeField]
    public UnityEvent OnFlappingWings { get; private set; } = new UnityEvent();


    public void SetInitialBehaviour() => OnSetInitialBehaviour.Invoke();

    public void FlappingWings() => OnFlappingWings.Invoke();
}