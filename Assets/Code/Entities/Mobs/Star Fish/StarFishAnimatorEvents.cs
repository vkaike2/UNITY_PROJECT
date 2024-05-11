using UnityEngine;
using UnityEngine.Events;

public class StarFishAnimatorEvents : MonoBehaviour
{
    [field: SerializeField]
    public UnityEvent OnSetInitialBehaviour { get; private set; } = new UnityEvent();
    [field: SerializeField]
    public UnityEvent OnSpawnProjectile {get; private set;} = new UnityEvent();

    public void SetInitialBehaviour() => OnSetInitialBehaviour.Invoke();
    public void SpawnProjectile() => OnSpawnProjectile.Invoke();
}