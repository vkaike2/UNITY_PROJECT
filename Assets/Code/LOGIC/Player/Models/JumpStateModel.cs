
using System;
using UnityEngine;

[Serializable]
public class JumpStateModel
{
    [SerializeField]
    private float _jumpForce = 6;
    [SerializeField]
    [Range(0, 1)]
    private float _cdwJumpAceleration = 0.4f;
    [SerializeField]
    private float _gravityFalling = 3;
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

    [Serializable]
    public class RaycastModel
    {
        [SerializeField]
        private float _colliderRadius = 0.9f;
        [SerializeField]
        private Transform _transform;


        public float ColliderRadius => _colliderRadius;
        public Transform GroundRaycastTransform => _transform;

        public void DrawGizmos(Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawWireCube(_transform.position, new Vector3(_colliderRadius, _colliderRadius / 2, 0));
        }

        public Collider2D DrawPhysics2D(LayerMask colisionLayer)
        {
            return Physics2D.OverlapArea(
                new Vector2(_transform.position.x - _colliderRadius / 2, _transform.position.y - _colliderRadius / 4),
                new Vector2(_transform.position.x + _colliderRadius / 2, _transform.position.y + _colliderRadius / 4),
                colisionLayer);
        }
    }
}
