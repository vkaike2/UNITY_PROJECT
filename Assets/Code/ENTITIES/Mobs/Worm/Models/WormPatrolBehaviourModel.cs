
using Assets.Code.LOGIC;
using System;
using UnityEngine;
using static WormPatrolBehaviour;

[Serializable]
public class WormPatrolBehaviourModel{
    [Header("idle state")]
    [SerializeField]
    private float _cdwBetweenWalks = 2f;

    [Space]
    [Header("patrol state")]
    [SerializeField]
    private float _patrolDistance = 2f;
    [Space]
    [SerializeField]
    private LayerMask _collidingWithLayerMask;
    [SerializeField]
    [Tooltip("offset distance that the worm will stop in each patrolling")]
    private float _distanceToStopPatrolling = 0.1f;
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

    public bool WillCollideWithGround => _willColideWithGround.IsCollidingWithLayer;

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
                    CurrentDirection == WormPatrolBehaviour.PatrolDirection.Right ?
                        _initialRaycastPosition.position.x + _patrolDistance :
                        _initialRaycastPosition.position.x - _patrolDistance,
                    _initialRaycastPosition.position.y));
        }
    }

    public RaycastHit2D DrawPatrolRaycast(WormPatrolBehaviour.PatrolDirection direction) {
        RaycastHit2D hit = Physics2D.Raycast(
                   _initialRaycastPosition.position,
                   direction == PatrolDirection.Right ? Vector2.right : Vector2.left,
                   _patrolDistance,
                   _collidingWithLayerMask);

        return hit;
    }

}
