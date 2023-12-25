using System;
using UnityEngine;
using UnityEngine.Events;
using static EnemyFollowingBehavior;

[Serializable]
public class EnemyFollowModel : EnemyFollowEventsModel
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float CdwBeforeJump { get; private set; } = 0.3f;
    [field: SerializeField]
    public float DistanceToStopFollow { get; private set; } = 2f;

    [field: Header("LAYERS")]
    [field: Space]
    [field: SerializeField]
    public LayerMask GroundLayer { get; private set; }

    [field: SerializeField]
    public LayerMask PlatformLayer { get; private set; }

    [field: Header("RAYCASTS CHECK")]
    [field: Space]
    [field: SerializeField]
    public RaycastModel GroundCheck { get; private set; }

    [field: Space(5)]
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public BoxCollider2D MainCollider { get; private set; }


    [HideInInspector]
    public Vector2 pointA;
    [HideInInspector]
    public Vector2 pointB;

    public bool draw = false;
}


