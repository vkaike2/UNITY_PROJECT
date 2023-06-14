using UnityEngine;
using UnityEngine.Events;

public class ChickenAnimatorEvents : MonoBehaviour
{
    [Header("EVENTS")]
    [SerializeField]
    private UnityEvent _onSetInitialBehaviour;
    [SerializeField]
    private UnityEvent _onInteractWithWorm;
    [SerializeField]
    private UnityEvent _onEndAtkWithWorm;
    

    public void SetInitialBehaviour() => _onSetInitialBehaviour.Invoke();

    public void InteractWithWorm() => _onInteractWithWorm.Invoke();

    public void EndAtkWormAnimation() => _onEndAtkWithWorm.Invoke();
}
