using UnityEngine;
using UnityEngine.Events;

public class ScorpionAnimatorEvents : MonoBehaviour
{
    [field: SerializeField]
    public UnityEvent OnSetInitialBehaviour { get; private set; } = new UnityEvent();

    public void SetInitialBehaviour() => OnSetInitialBehaviour.Invoke();
}