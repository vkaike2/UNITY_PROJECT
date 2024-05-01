using UnityEngine;
using UnityEngine.Events;

public class ArmadilloAnimatorEvents : MonoBehaviour
{
    [field: Header("EVENTS")]
    [field: SerializeField]
    public UnityEvent OnFinishSwpawnAnimation { get; set; } = new UnityEvent();
    [field: SerializeField]
    public UnityEvent OnSpawnShockwave {get; set;} = new UnityEvent();

    public void FinishSwpawnAnimation() => OnFinishSwpawnAnimation.Invoke();

    public void SpawnShockwave() => OnSpawnShockwave.Invoke();
}