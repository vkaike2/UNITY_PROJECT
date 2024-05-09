using System;
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

    public bool CanAttack { get; set; } = true;
}