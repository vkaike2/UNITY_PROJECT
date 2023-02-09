using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestPath : MonoBehaviour
{

    public bool isFollowing = true;
    public float distanceToFollow = 50f;
    public float updateCdw = 0.5f;
    public float speed = 3;
    public float nextWaypointDistance = 3f;
    public LayerMask layerMask;
    public float stopFollowDistance = 1f;


    [Space]
    public Transform target;

    private Path _path;
    private int _curentWaypont = 0;
    private Seeker _seeker;
    private Rigidbody2D _rigidbody2D;

    private Vector2 _waypointPosition;

    private void OnDrawGizmos()
    {
        if (_waypointPosition == Vector2.zero) return;

        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawLine(_waypointPosition, new Vector2(_waypointPosition.x, _waypointPosition.y - 1f));
    }

    private void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdatePath), 0f, updateCdw);
    }

    private void FixedUpdate()
    {
        if (IsTargetInDistance() && isFollowing)
        {
            FollowTarget();
        }
    }

    private void FollowTarget()
    {
        if (_path == null) return;
        if (_curentWaypont >= _path.vectorPath.Count) return;
        if (Vector2.Distance(transform.position, target.position) < stopFollowDistance) return;

        _waypointPosition = _path.vectorPath[_curentWaypont];

        Vector2 direction = (_waypointPosition - _rigidbody2D.position).normalized;

        _rigidbody2D.velocity = new Vector2(direction.x * speed, _rigidbody2D.velocity.y);

        float distance = Vector2.Distance(_rigidbody2D.position, _path.vectorPath[_curentWaypont]);
        if (distance < nextWaypointDistance)
        {
            _curentWaypont++;
        }
    }

    private bool IsTargetInDistance()
    {
        return Vector2.Distance(transform.position, target.position) < distanceToFollow;
    }

    private void UpdatePath()
    {
        if (isFollowing && IsTargetInDistance() && _seeker.IsDone())
        {
            _seeker.StartPath(_rigidbody2D.position, target.position, (Path p) =>
            {
                if (p.error) return;

                if (WillFall(p))
                {
                    return;
                }

                _path = p;
                _curentWaypont = 0;

            });
        }
    }

    private bool WillFall(Path completePath)
    {
        foreach (Vector3 pathPosition in completePath.vectorPath)
        {
            if (!WillCollideWithGround(pathPosition))
            {
                return true;
            }
        }
        return false;
    }

    private bool WillCollideWithGround(Vector2 position)
    {
        RaycastHit2D col = Physics2D.Linecast(position, new Vector2(position.x, position.y - 1f), layerMask);
        return col.collider != null;
    }
}
