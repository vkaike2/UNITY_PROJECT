using Assets.Code.LOGIC;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPatrolModel
{
    [field: Header("IDLE STATE")]
    [field: SerializeField]
    public float CdwBetweenWalks { get; private set; } = 2f;

    [field: Space]
    [field: Header("PATROL STATE")]
    [field: SerializeField]
    public float PatrolDistance { get; private set; } = 4f;
    [field: Space]
    [field: SerializeField]
    public LayerMask CollidingWithLayerMask { get; private set; }
    [SerializeField]
    [Tooltip("offset distance that the enemy will stop in each patrolling")]
    public float DistanceToStopPatrolling { get; private set; } = 0.3f;
    [field: SerializeField]
    public Transform InitialRaycastPosition { get; private set; }
    [SerializeField]
    private LayerCheckCollider _willColideWithGround;

    public OnChangeAnimationEvent OnChangeAnimation { get; private set; } = new OnChangeAnimationEvent();

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
                InitialRaycastPosition.position,
                new Vector2(
                    CurrentDirection == PatrolDirection.Right ?
                        InitialRaycastPosition.position.x + PatrolDistance :
                        InitialRaycastPosition.position.x - PatrolDistance,
                    InitialRaycastPosition.position.y));
        }
    }
    
    public RaycastHit2D DrawPatrolRaycast(PatrolDirection direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(
                   InitialRaycastPosition.position,
                   direction == PatrolDirection.Right ? Vector2.right : Vector2.left,
                   PatrolDistance,
                   CollidingWithLayerMask);

        return hit;
    }

    public enum PossibleAnimations
    {
        Idle, Move
    }

    public enum PatrolDirection
    {
        Left,
        Right
    }

    public class OnChangeAnimationEvent : UnityEvent<PossibleAnimations> { }
}
