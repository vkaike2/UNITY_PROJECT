using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyPatrolModel;

public partial class EnemyPatrolBehaviour
{
    public class Patrol : EnemyBaseBehaviour
    {
        private EnemyPatrolModel _model;
        private Coroutine _waitingCoroutine = null;
        private Coroutine _patrolCoroutine = null;
        private Vector2 _currentPatrolEndPosition;

        private readonly List<PatrolDirection> _directions = new List<PatrolDirection>()
        {
            PatrolDirection.Left, 
            PatrolDirection.Right
        };

        public Patrol(EnemyPatrolModel patrolModel)
        {
            _model = patrolModel;
        }

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
        }

        public override void OnEnterBehaviour()
        {
            _waitingCoroutine = _enemy.StartCoroutine(WaitForCdw());
        }

        public override void OnExitBehaviour()
        {
            _rigidbody2D.isKinematic = false;
            _model.OnChangeAnimation.RemoveAllListeners();
            ResetCoroutines();
            DisableGizmo();
        }

        public override void Update() { }

        private void DisableGizmo() => _model.DisablePatrolGizmos();

        private void ResetCoroutines()
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

        private bool CheckIfCanPatrolToThisDirection(PatrolDirection direction)
        {
            RaycastHit2D hit = _model.DrawPatrolRaycast(direction);
            return hit.collider == null;
        }

        private PatrolDirection? GetDirection(List<PatrolDirection> possibleDirections = null)
        {
            if (possibleDirections == null)
            {
                possibleDirections = new List<PatrolDirection>();
                possibleDirections.AddRange(_directions);
            }

            if (possibleDirections.Count == 0 )
            {
                return null;
            }

            PatrolDirection currentDirection = possibleDirections[UnityEngine.Random.Range(0, possibleDirections.Count)];

            if (CheckIfCanPatrolToThisDirection(currentDirection))
            {
                return currentDirection;
            }

            possibleDirections.Remove(currentDirection);

            return GetDirection(possibleDirections);
        }

        private Vector2 CalculateNextMove()
        {
            PatrolDirection? direction = GetDirection();
            Vector2 nextMoveDirection;

            if (direction == null)
            {
                _waitingCoroutine = _enemy.StartCoroutine(WaitForCdw());
                return Vector2.zero;
            }
            else
            {
                _currentPatrolEndPosition =
                    new Vector2(
                        direction == PatrolDirection.Right ? _enemy.transform.position.x + _model.PatrolDistance : _enemy.transform.transform.position.x - _model.PatrolDistance,
                        _enemy.transform.position.y);

                nextMoveDirection = new Vector2(direction == PatrolDirection.Right ? 1 : -1, 0);

                _enemy.RotationalTransform.localScale = new Vector3(direction == PatrolDirection.Right ? 1 : -1, 1, 1);

                _enemy.StartCoroutine(DrawPatrolGizmoForSeconds(direction.Value));
            }

            return nextMoveDirection;
        }

        private IEnumerator WaitForCdw()
        {
            _model.OnChangeAnimation.Invoke(PossibleAnimations.Idle);
            _rigidbody2D.velocity = Vector3.zero;
            _rigidbody2D.isKinematic = true;

            yield return new WaitForSeconds(_model.CdwBetweenWalks);


            Vector2 nextMove = CalculateNextMove();
            if (nextMove != Vector2.zero)
            {
                _rigidbody2D.isKinematic = false;
                _patrolCoroutine = _enemy.StartCoroutine(ManagePatrol(nextMove));
            }
        }

        IEnumerator ManagePatrol(Vector2 nextDirection)
        {
            _model.OnChangeAnimation.Invoke(PossibleAnimations.Move);
            Vector2 myHorizontalPosition = new Vector2(_enemy.transform.position.x, 0);
            Vector2 patrolHorizontalPosition = new Vector2(_currentPatrolEndPosition.x, 0);

            while (Vector2.Distance(myHorizontalPosition, patrolHorizontalPosition) >= _model.DistanceToStopPatrolling)
            {
                if (_enemy.CanMove)
                {
                    if (!_model.WillCollideWithGround)
                    {
                        break;
                    }

                    _rigidbody2D.velocity = nextDirection * _enemy.Status.MovementSpeed.Get();
                }
                else
                {
                    _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
                }

                myHorizontalPosition = new Vector2(_enemy.transform.position.x, 0);
                yield return new WaitForFixedUpdate();
            }

            _rigidbody2D.velocity = Vector2.zero;

            _waitingCoroutine = _enemy.StartCoroutine(WaitForCdw());
        }

        private IEnumerator DrawPatrolGizmoForSeconds(PatrolDirection direction)
        {
            _model.SetupPatrolGizmos(direction);
            yield return new WaitForSeconds(2f);
            _model.DisablePatrolGizmos();
        }
    }
}
