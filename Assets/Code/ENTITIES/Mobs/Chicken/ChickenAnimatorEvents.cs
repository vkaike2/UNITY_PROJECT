using UnityEngine;
using UnityEngine.Events;

public class ChickenAnimatorEvents : MonoBehaviour
{
    [field: Header("EVENTS")]
    [field: SerializeField]
    public UnityEvent OnSetInitialBehaviour { get; private set; } = new UnityEvent();
    [field: SerializeField]
    public UnityEvent OnInteractWithTarget { get; private set; } = new UnityEvent();
    [field: SerializeField]
    public UnityEvent OnEndMeleeAtk { get; private set; } = new UnityEvent();
    [field: Space]
    [field: SerializeField]
    public UnityEvent OnThrowingEgg { get; private set; } = new UnityEvent();

    public void SetInitialBehaviour() => OnSetInitialBehaviour.Invoke();

    public void InteractWithTarget()
    {
        OnInteractWithTarget.Invoke();
    }

    public void EndMeleeAtkAnimation() => OnEndMeleeAtk.Invoke();

    public void ThrowingEgg() => OnThrowingEgg.Invoke();
}
