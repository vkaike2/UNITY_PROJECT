
using System;
using UnityEngine;

[Serializable]
public class PlayerJumpModel
{
    [field: SerializeField]
    [field: Range(0, 1)]
    [field: Tooltip("amount of time to go from 0 to 100% jump force")]
    public float CdwJumpAceleration { get; private set; } = 0.4f;

    [field: SerializeField]
    [field: Tooltip("Time where you can jup after leaving a platform")]
    public float CoyoteTime { get; private set; } = 0.1f;

    [field: Header("RAYCAST CHECKS")]
    [field: Space]
    [field: SerializeField]
    public LayerMask GroundLayer { get; private set; }
    
    [field: SerializeField]
    public RaycastModel GroundCheck { get; private set; }

    [field: SerializeField]
    [field: Tooltip("Buffer that checks if you can jump even when you are not touching the ground")]
    public RaycastModel BufferCheck { get; private set; }

    public bool IsLandingOnAPlatform { get; set; } = false;
    public bool IsCoyoteTimeActive { get; set; } = false;
}
