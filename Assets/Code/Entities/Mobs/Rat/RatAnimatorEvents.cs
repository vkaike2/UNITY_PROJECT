using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RatAnimatorEvents : MonoBehaviour
{
    [field: SerializeField]
    public UnityEvent OnSetInitialBehaviour { get; private set; } = new UnityEvent();


    public void SetInitialBehaviour() => OnSetInitialBehaviour.Invoke();
}
