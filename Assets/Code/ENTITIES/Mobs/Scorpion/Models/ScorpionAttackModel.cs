using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ScorpionAttackModel
{
    [field: Header("PREFAB")]
    [field: SerializeField]
    public ScorpionProjectile Projectile { get; private set; }

    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public ScorpionTail Tail { get; private set; }

    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float CdwBetweenShoot { get; private set; }
    [field: SerializeField]
    public float ProjectileSpeed { get; private set; }


    public UnityEvent OnReadyToShootAgain { get; private set; } = new UnityEvent();
}