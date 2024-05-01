
using System;
using UnityEngine;

[Serializable]
public class ArmadilloIntoBallModel
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float Velocity { get; set; } = 5;
    [field: SerializeField]
    public float CdwToStartBouncing { get; set; } = 2;
    [field: SerializeField]
    public float Duration { get; set; } = 5;

    [field: Header("CHECKS")]
    [field: SerializeField]
    public LayerCheckCollider CheckIfWillHitTheFloor { get; set; }
}