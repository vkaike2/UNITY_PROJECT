
using System;
using UnityEngine;

[Serializable]
public class PlayerJumpStateModel
{
    [SerializeField]
    private float _jumpForce = 8.5f;
    [SerializeField]
    [Range(0, 1)]
    private float _cdwJumpAceleration = 0.4f;
    [SerializeField]
    private float _gravityFalling = 4;
    [SerializeField]
    private float _coyoteTime = 0.1f;

    [Space]
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private RaycastModel _groundCheck;
    [SerializeField]
    private RaycastModel _bufferCheck;

    public float JumpForce => _jumpForce;
    public float CdwJumpAceleration => _cdwJumpAceleration;
    public float GravityFalling => _gravityFalling;
    public float CoyoteTime => _coyoteTime;

    public LayerMask GroundLayer => _groundLayer;

    public RaycastModel GroundCheck => _groundCheck;
    public RaycastModel BufferCheck => _bufferCheck;    
}
