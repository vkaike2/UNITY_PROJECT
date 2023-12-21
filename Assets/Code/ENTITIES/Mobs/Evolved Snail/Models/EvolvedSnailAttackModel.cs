using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EvolvedSnailAttackModel
{
    [field: Header("PREFAB")]
    [field: SerializeField]
    public EvolvedSnailProjectile Projectile { get; private set; }
    [field: SerializeField]
    public Transform ButtPosition { get; private set; }

    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float PercentageGravityMultiplier { get; private set; } = 0.1f;
    [field: SerializeField]
    public float ProjectileSpeed { get; private set; } = 7f;
    [field: SerializeField]
    public float CdwBetweenEachAttack { get; set; } = 3f;

    public bool CanAttack { get; set; } = true;

    // called when animation is exacly at the attack frame
    public UnityEvent OnAttackFrame { get; private set; } = new UnityEvent();
}