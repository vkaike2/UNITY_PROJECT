using UnityEngine;
using UnityEngine.Events;

public class ChickenAnimatorEvents : MonoBehaviour
{
    [Header("EVENTS")]
    [SerializeField]
    private UnityEvent _onSetInitialBehaviour;
    [SerializeField]
    private UnityEvent _onInteractWithTarget;
    [SerializeField]
    private UnityEvent _onEndMeleeAtk;
    [Space]
    [SerializeField]
    private UnityEvent _onThrowingEgg;

    public void SetInitialBehaviour() => _onSetInitialBehaviour.Invoke();

    public void InteractWithWorm() => _onInteractWithTarget.Invoke();

    public void EndMeleeAtkAnimation() => _onEndMeleeAtk.Invoke();

    public void ThrowingEgg() => _onThrowingEgg.Invoke();
}
