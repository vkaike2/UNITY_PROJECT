using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ChickenFollowingBehaviourModel
{
    [Header("COMPONENTS")]
    [SerializeField]
    private float _cdwBeforeJump = 0.3f;

    [Header("LAYERS")]
    [Space]
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private LayerMask _wallLayer;
    [SerializeField]
    private LayerMask _platformLayer;

    [Space]
    [SerializeField]
    private RaycastModel _groundCheck;

    [Space(5)]
    [SerializeField]
    private RaycastModel _wallCheck;


    public float CdwBeforeJump => _cdwBeforeJump;
    public LayerMask WallLayer => _wallLayer;
    public LayerMask GroundLayer => _groundLayer;
    public LayerMask PlatformLayer => _platformLayer;
    public RaycastModel GroundCheck => _groundCheck;
    public RaycastModel WallCheck => _wallCheck;


    public Vector2 pointA;
    public Vector2 pointB;
    public bool draw = false;
    public void GizmosTest()
    {
        if(!draw)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pointA, pointB);
    }
}
