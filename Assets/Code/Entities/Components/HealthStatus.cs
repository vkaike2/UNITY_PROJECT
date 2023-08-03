using UnityEngine;

public class HealthStatus : MonoBehaviour
{
    [field: Header("HEALTH")]
    [field: SerializeField]
    public StatusFloatAttribute MaxHealth { get; private set; }
    [field: Space] 
    [field: SerializeField]
    public float KnockBackForce { get; private set; } = 700;

    [HideInInspector]
    public StatusFloatAttribute Health;


    public void InitializeHealth()
    {
        Health.Set(MaxHealth.Get());
    }
}
