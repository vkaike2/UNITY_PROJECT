using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SeagullFlyModel
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float FlyDuration { get; set; }
    [field: SerializeField]
    public float CdwAttack { get; set; }
    [field: SerializeField]
    public float VerticalVelocity { get; set; } = 2.5f;
    [field: SerializeField]
    public float VeriticalVelocityToGoUp { get; set; } = 4f;

    [field: Header("POSITIONS")]
    [field: SerializeField]
    public Transform PoopPosition { get; set; }

    [field: Header("PREFAB")]
    [field: SerializeField]
    public SeagullProjectile SeagullProjectilePrefab { get; set; }

    [field: Header("CHECKS")]
    [field: SerializeField]
    public LayerCheckCollider CheckIfImAboveGround { get; set; }
    [field: SerializeField]
    public LayerCheckCollider CheckIsTouchingGround { get; set; }

    public UnityEvent OnFlappingWings { get; private set; } = new UnityEvent();
    public float InitialVerticalPosition { get; set; }

}