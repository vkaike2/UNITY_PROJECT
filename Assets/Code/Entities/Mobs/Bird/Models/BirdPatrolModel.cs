using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BirdPatrolModel
{
    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float HorizontalVelocity { get; private set; } = 5f;
    [field: SerializeField]
    public MinMax AtkCdw { get; private set; }

    [field: Space]
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public LayerCheckCollider CollidingWithWall { get; private set; }

    [field: Header("UI COMPONENTS")]
    [field:SerializeField]
    public CdwIndicationUI CdwIndicatorUI { get; private set; }

    public float InitialHorizontalPosition { get; set; }

    public UnityEvent OnFlappingWings { get; private set; } = new UnityEvent();
}
