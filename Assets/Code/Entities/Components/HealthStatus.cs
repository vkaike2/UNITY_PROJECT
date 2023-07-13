using UnityEngine;

public class HealthStatus : MonoBehaviour
{
    [Header("HEALTH")]
    public StatusFloatAttribute MaxHealth;
    [SerializeField]
    private float _knockBackForce = 700;
    [HideInInspector]
    public StatusFloatAttribute Health;

    public float KnockBackForce => _knockBackForce;

    public void InitializeHealth()
    {
        Health.Set(MaxHealth.Get());
    }
}
