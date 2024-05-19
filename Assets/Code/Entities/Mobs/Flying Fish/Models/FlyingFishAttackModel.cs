using System;
using UnityEngine;

[Serializable]
public class FlyingFishAttackModel
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float FlyingGravity { get; set; }
    [field: SerializeField]
    public MinMax CdwToAttack { get; set; } = new MinMax(3, 4);
    [field: SerializeField]
    public float JumpForce { get; set; }
    [field: SerializeField]
    public float FloorDuration { get; set; }

    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public Collider2D MainCollider { get; set; }

    public bool IsUnderTheWater { get; set; } = false;

    public bool IsReadyToAttack { get; set; } = false;
}