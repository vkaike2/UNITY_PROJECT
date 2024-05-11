using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SharkAttackModel
{
    [field: Header("CHECKS")]
    [field: SerializeField]
    public LayerCheckCollider PlayerOnRangeCheck { get; set; }

    [field: Header("EVENTS")]
    [field: SerializeField]
    public UnityEvent OnAttackFinished { get; set; } = new UnityEvent();

    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float AttakCdw { get; set; } = 3f;

    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public Collider2D AttackHitboxCollider { get; set; }

    public bool CanAttack { get; set; } = true;
}