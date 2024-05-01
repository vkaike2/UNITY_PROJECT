using System;
using UnityEngine;

[Serializable]
public class ArmadilloRunningTowardsWallModel
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float Speed { get; set; } = 7;
    [field: SerializeField]
    public float JumpForce { get; set; }
    [field: SerializeField]
    public float StunDuration { get; set; } = 2f;

    [field: Header("CHECKS")]
    [field: SerializeField]
    public LayerCheckCollider WillHitTheWallCheck { get; set; }
    [field: SerializeField]
    public LayerCheckCollider AlreadyHitTheWallCheck { get; set; }
}