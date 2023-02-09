using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WormPatrolBehaviour : WormFiniteBaseBehaviour
{
    public override Worm.Behaviour Behaviour => Worm.Behaviour.Patrol;

    private WormPatrolBehaviourModel _patrolModel;
    private Vector2 _currentPatrolEndPosition;

    private Coroutine _waitingCoroutine = null;
    private Coroutine _patrolCoroutine = null;

    private readonly List<PatrolDirection> _directions = new List<PatrolDirection>()
    {
        PatrolDirection.Left, PatrolDirection.Right
    };

    public override void OnEnterBehaviour()
    {
        _waitingCoroutine = _worm.StartCoroutine(Wainting());

        StartCheckIfPlayerIsInRange();
    }

    public override void OnExitBehaviour()
    {
        if (_waitingCoroutine != null)
        {
            _worm.StopCoroutine(_waitingCoroutine);
        }
        if (_patrolCoroutine != null)
        {
            _worm.StopCoroutine(_patrolCoroutine);
        }

        _patrolModel.DisablePatrolGizmos();
    }

    public override void Start(Worm worm)
    {
        base.Start(worm);
        _patrolModel = worm.PatrolModel;
    }

    public override void Update()
    {
        if (_mySeeker.IsTargetInRange)
        {
            _worm.ChangeBehaviour(Worm.Behaviour.FollowingPlayer);
        }
    }

    IEnumerator Wainting()
    {
        _worm.Animator.PlayAnimation(WormAnimatorModel.Animation.Idle);
        _worm.CanMove = true;
        yield return new WaitForSeconds(_patrolModel.CdwBetweenWalks);

        Vector2 nextMove = CalculateNextMove();
        if (nextMove != Vector2.zero)
        {
            _patrolCoroutine = _worm.StartCoroutine(Patrol(nextMove));
        }
    }

    IEnumerator Patrol(Vector2 nextDirection)
    {
        _worm.Animator.PlayAnimation(WormAnimatorModel.Animation.Move);

        while (Vector2.Distance(_worm.transform.position, _currentPatrolEndPosition) >= _patrolModel.DistanceToStopPatrolling)
        {
            if (_worm.CanMove)
            {
                if (!_patrolModel.WillCollideWithGround)
                {
                    break;
                }

                _rigidbody2D.velocity = nextDirection * _worm.MovementSpeed;
            }
            yield return new WaitForFixedUpdate();
        }
        _rigidbody2D.velocity = Vector2.zero;

        _waitingCoroutine = _worm.StartCoroutine(Wainting());
    }

    private Vector2 CalculateNextMove()
    {
        PatrolDirection? direction = GetDirection();
        Vector2 nextMoveDirection;

        if (direction == null)
        {
            _waitingCoroutine = _worm.StartCoroutine(Wainting());
            return Vector2.zero;
        }
        else
        {
            _currentPatrolEndPosition =
                new Vector2(
                    direction == PatrolDirection.Right ? _worm.transform.position.x + _patrolModel.PatrolDistance : _worm.transform.transform.position.x - _patrolModel.PatrolDistance,
                    _worm.transform.position.y);
            nextMoveDirection = new Vector2(direction == PatrolDirection.Right ? 1 : -1, 0);

            _worm.RotationalTransform.localScale = new Vector3(direction == PatrolDirection.Right ? 1 : -1, 1, 1);

            _worm.StartCoroutine(DrawPatrolGizmoForSeconds(direction.Value));
        }

        return nextMoveDirection;
    }

    private IEnumerator DrawPatrolGizmoForSeconds(PatrolDirection direction)
    {
        _patrolModel.SetupPatrolGizmos(direction);
        yield return new WaitForSeconds(2f);
        _patrolModel.DisablePatrolGizmos();
    }

    private PatrolDirection? GetDirection(List<PatrolDirection> excluded = null)
    {
        if (excluded == null)
        {
            excluded = new List<PatrolDirection>();
            excluded.AddRange(_directions);
        }

        if (excluded.Count == 0)
        {
            return null;
        }

        PatrolDirection currentDirection = excluded[Random.Range(0, excluded.Count)];

        if (CheckIfCanPatrolToThisDirection(currentDirection))
        {
            return currentDirection;
        }

        excluded.Remove(currentDirection);

        return GetDirection(excluded);
    }

    private bool CheckIfCanPatrolToThisDirection(PatrolDirection direction)
    {
        RaycastHit2D hit = _patrolModel.DrawPatrolRaycast(direction);
        return hit.collider == null;
    }

    public enum PatrolDirection
    {
        Left,
        Right
    }

    /// <summary>
    /// It stops automatically when player is in range
    /// </summary>
    private void StartCheckIfPlayerIsInRange()
    {
        if (_gameManager == null || _gameManager.Player == null) return;

        _worm.StartCoroutine(
            _mySeeker.CheckIfTargetInRange(
                MySeeker.AvailableMovements.Walk,
                _worm.gameObject,
                _gameManager.Player.gameObject));

    }
}
