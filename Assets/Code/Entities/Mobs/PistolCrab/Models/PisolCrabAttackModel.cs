using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PistolCrabAttackModel
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float AttackDuration { get; set; } = 5;
    [field: SerializeField]
    public float CdwToAttack { get; set; }
    [field: SerializeField]
    public float DistanceToAttackPlayer { get; set; } = 2;
    [field: Space]
    [field: SerializeField]
    public MinMax ProjectileSpeed { get; set; }

    [field: Header("CHECKS")]
    [field: SerializeField]
    public LayerCheckCollider PlayerOnRangeCheck { get; set; }

    [field: Header("POSITIONS")]
    [field: SerializeField]
    public Transform LeftPistolPosition { get; set; }
    [field: SerializeField]
    public Transform RightPistolPosition { get; set; }

    [field: Header("PREFAB")]
    [field: SerializeField]
    public PistolCrabProjectile ProjectilePrefab { get; set; }


    public UnityEvent OnShootLeftPistol { get; set; } = new UnityEvent();
    public UnityEvent OnShootRightPistol { get; set; } = new UnityEvent();

    public bool CanAttack { get; set; } = false;
}