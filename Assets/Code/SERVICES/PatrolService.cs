using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;


public class PatrolService
{
    private Enemy _enemy;
    private EnemyPatrolModel _patrolModel;
    private Action _onChangeAnimationToIdle;
    private Action _onChangeAnimationToMove;
    private Rigidbody2D _rigidbody2D;

    private Vector2 _currentPatrolEndPosition;
    private Coroutine _waitingCoroutine = null;
    private Coroutine _patrolCoroutine = null;

    private readonly List<PatrolDirection> _directions = new List<PatrolDirection>()
    {
        PatrolDirection.Left, PatrolDirection.Right
    };

    public PatrolService(Enemy enemy, EnemyPatrolModel patrolModel, Action onChangeAnimationToIdle, Action onChangeAnimationToMove)
    {
        _patrolModel = patrolModel;
        _enemy = enemy;
        _rigidbody2D = _enemy.GetComponent<Rigidbody2D>();

        _onChangeAnimationToIdle = onChangeAnimationToIdle;
        _onChangeAnimationToMove = onChangeAnimationToMove;
    }


    /// <summary>
    ///     - wait for a patroll cdw
    ///     - calculate next movement
    ///     - move
    ///     - Repeat
    /// </summary>
    public void StartPatrolBehaviour()
    {
        _waitingCoroutine = _enemy.StartCoroutine(Wainting());
    }

    public void ResetCoroutines()
    {
        if (_waitingCoroutine != null)
        {
            _enemy.StopCoroutine(_waitingCoroutine);
        }
        if (_patrolCoroutine != null)
        {
            _enemy.StopCoroutine(_patrolCoroutine);
        }
    }

    public void DisabelGizmo() => _patrolModel.DisablePatrolGizmos();

    IEnumerator Wainting()
    {
        _onChangeAnimationToIdle?.Invoke();

        //_enemy.CanMove = true;
        yield return new WaitForSeconds(_patrolModel.CdwBetweenWalks);

        Vector2 nextMove = CalculateNextMove();
        if (nextMove != Vector2.zero)
        {
            _patrolCoroutine = _enemy.StartCoroutine(Patrol(nextMove));
        }
    }

    private Vector2 CalculateNextMove()
    {
        PatrolDirection? direction = GetDirection();
        Vector2 nextMoveDirection;

        if (direction == null)
        {
            _waitingCoroutine = _enemy.StartCoroutine(Wainting());
            return Vector2.zero;
        }
        else
        {
            _currentPatrolEndPosition =
                new Vector2(
                    direction == PatrolDirection.Right ? _enemy.transform.position.x + _patrolModel.PatrolDistance : _enemy.transform.transform.position.x - _patrolModel.PatrolDistance,
                    _enemy.transform.position.y);
            nextMoveDirection = new Vector2(direction == PatrolDirection.Right ? 1 : -1, 0);

            _enemy.RotationalTransform.localScale = new Vector3(direction == PatrolDirection.Right ? 1 : -1, 1, 1);

            _enemy.StartCoroutine(DrawPatrolGizmoForSeconds(direction.Value));
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

        PatrolDirection currentDirection = excluded[UnityEngine.Random.Range(0, excluded.Count)];

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

    IEnumerator Patrol(Vector2 nextDirection)
    {
        _onChangeAnimationToMove?.Invoke();

        while (Vector2.Distance(_enemy.transform.position, _currentPatrolEndPosition) >= _patrolModel.DistanceToStopPatrolling)
        {
            if (_enemy.CanMove)
            {
                Debug.Log("patrol ?");
                if (!_patrolModel.WillCollideWithGround)
                {
                    break;
                }

                _rigidbody2D.velocity = nextDirection * _enemy.Status.MovementSpeed.Get();
            }
            else
            {
                _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            }
            yield return new WaitForFixedUpdate();
        }

        _rigidbody2D.velocity = Vector2.zero;

        _waitingCoroutine = _enemy.StartCoroutine(Wainting());
    }


    public enum PatrolDirection
    {
        Left,
        Right
    }
}
