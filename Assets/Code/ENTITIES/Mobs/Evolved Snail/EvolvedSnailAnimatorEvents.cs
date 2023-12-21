using UnityEngine;
using UnityEngine.Events;

public class EvolvedSnailAnimatorEvents : MonoBehaviour
{
    [field: Header("EVENTS")]
    [field: SerializeField]
    public UnityEvent OnSetInitialBehaviour { get; private set; } = new UnityEvent();
    [field: Space]
    [field: SerializeField]
    public UnityEvent OnAttackFrame { get; private set; } = new UnityEvent();


    public void SetInitialBehaviour() => OnSetInitialBehaviour.Invoke();

    public void AttackFrame() => OnAttackFrame.Invoke();
}