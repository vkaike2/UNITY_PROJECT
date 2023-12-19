using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class BatPatrolModel
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float VerticalSpeed { get; private set; } = 3.4f;
    [field: SerializeField]
    public float GravityScale { get; private set; } = 1f;
    [field: Space]
    [field: SerializeField]
    public float PatrolDistance { get; private set; } = 2f;
    [field: SerializeField]
    public LayerMask GroundLayer { get; private set; }
    [field: SerializeField]
    public float CdwBetweenWalks { get; private set; } = 2f;
    [field: Space]
    [field: SerializeField]
    public float DistanceToStartFollowPlayer { get; set; }


    public Vector2? NewPosition { get; set; } = null;

    public void DrawGizmos(Vector2 myPosition)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(myPosition, DistanceToStartFollowPlayer);

        if (NewPosition == null) return;
        Gizmos.color = Color.red; // Set the color of the circle
        Gizmos.DrawWireSphere(NewPosition.Value, 0.5f);
    }
}
