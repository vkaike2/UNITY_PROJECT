using UnityEngine;
using UnityEngine.Events;

public class ChickenAnimatorEvents : MonoBehaviour
{
    [Header("events")]
    [SerializeField]
    private UnityEvent _onSetInitialBehaviour;

    public void SetInitialBehaviour() => _onSetInitialBehaviour.Invoke();
}
