using System.Collections;
using UnityEngine;

public class PlayerStatus : HealthStatus
{
    [field: Header("DAMAGE")]
    [field: SerializeField]
    public StatusFloatAttribute PoopDamage { get; private set; }
    [field: SerializeField]
    public StatusFloatAttribute FartDamage { get; private set; }

    [field: Header("MOVEMENT")]
    [field: SerializeField]
    public StatusFloatAttribute MovementSpeed { get; private set; }
    [field: SerializeField]
    public StatusFloatAttribute JumpForce { get; private set; }
}
