using Assets.Code.LOGIC;
using UnityEngine;
using static PatrolService;

public class EnemyPatrolModel
{
    [Header("IDLE STATE")]
    [SerializeField]
    private float _cdwBetweenWalks = 2f;

    [Space]
    [Header("PATROL STATE")]
    [SerializeField]
    private float _patrolDistance = 4f;
    [Space]
    [SerializeField]
    private LayerMask _collidingWithLayerMask;
    [SerializeField]
    [Tooltip("offset distance that the enemy will stop in each patrolling")]
    private float _distanceToStopPatrolling = 0.3f;
    [SerializeField]
    private Transform _initialRaycastPosition;
    [SerializeField]
    private LayerCheckCollider _willColideWithGround;

    public float CdwBetweenWalks => _cdwBetweenWalks;

    public float PatrolDistance => _patrolDistance;
    public LayerMask CollidingWithLayerMask => _collidingWithLayerMask;
    public float DistanceToStopPatrolling => _distanceToStopPatrolling;

    public Transform InitialRaycastPosition => _initialRaycastPosition;

    public bool DrawPatrolGizmos { get; private set; }
    public PatrolDirection CurrentDirection { get; private set; }

    public bool WillCollideWithGround => _willColideWithGround.IsRaycastCollidingWithLayer;

    public void SetupPatrolGizmos(PatrolDirection direction)
    {
        CurrentDirection = direction;
        DrawPatrolGizmos = true;
    }
    public void DisablePatrolGizmos() => DrawPatrolGizmos = false;

    public void OnDrawGizmos()
    {
        if (DrawPatrolGizmos)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawLine(
                _initialRaycastPosition.position,
                new Vector2(
                    CurrentDirection == PatrolDirection.Right ?
                        _initialRaycastPosition.position.x + _patrolDistance :
                        _initialRaycastPosition.position.x - _patrolDistance,
                    _initialRaycastPosition.position.y));
        }
    }

    public RaycastHit2D DrawPatrolRaycast(PatrolDirection direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(
                   _initialRaycastPosition.position,
                   direction == PatrolDirection.Right ? Vector2.right : Vector2.left,
                   _patrolDistance,
                   _collidingWithLayerMask);

        return hit;
    }


}
