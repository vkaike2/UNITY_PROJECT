using System;
using UnityEngine;

[Serializable]
public class ScorpionWalkModel
{
    [field: Header("RAYCAST CHECKS")]
    [field: SerializeField]
    public LayerMask LayerMask { get; private set; }
    [field: Space]
    [field: SerializeField]
    public RaycastModel WillHitTheWallCheck { get; private set; }
    [field: SerializeField]
    public RaycastModel WillHitTheGround { get; private set; }


    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float CdwToMoveBackToIdle { get; set; }
}