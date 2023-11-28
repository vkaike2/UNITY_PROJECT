using Calcatz.MeshPathfinding;
using UnityEngine;


public partial class EnemyFollowingBehaviour : MonoBehaviour
{
    public class Walk : EnemyBaseBehaviour
    {
        private readonly EnemyFollowingBehavior _parent;
        private Vector2 _direction = Vector2.zero;
        private EnemyFollowEventsModel _followEventsModel;

        //If player is above you, you will try to follow him from the ground
        public const float DISTANCE_TO_STOP_FLLOWING_TARGET = 2;

        public Walk(EnemyFollowingBehavior parent)
        {
            _parent = parent;
            _followEventsModel = parent.EnemyFollowEventsModel;
        }

        public override void OnEnterBehaviour()
        {
            _followEventsModel.OnChangeAnimation.Invoke(EnemyFollowEventsModel.PossibleAnimations.Move);
            _parent.Pathfinding.StartFindPath(0);
        }

        public override void OnExitBehaviour()
        {
            _followEventsModel.ResetEvents();
            _parent.Pathfinding.StopPathFinding();
        }

        public override void Update()
        {
            if (!_enemy.CanMove)
            {
                _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
                return;
            };

            Node[] paths = _parent.Pathfinding.GetPathResult();

            Transform targetTransform;
            if (paths == null)
            {
                targetTransform = CheckIfCanFollowTarget();

                if (targetTransform == null)
                {
                    _followEventsModel.OnTargetUnreachable.Invoke();
                    return;
                }
            }
            else
            {
                targetTransform = GetFirstNodeToFollow(paths);
            }

            CalculateDirection(targetTransform);

            
            _enemy.RotationalTransform.localScale = new Vector3(_direction.x, 1, 1);
            _rigidbody2D.velocity = new Vector2(_direction.x * _enemy.Status.MovementSpeed.Get(), 0);
        }

        public Transform CheckIfCanFollowTarget()
        {
            Transform targetTransform = _parent.Pathfinding.Target.transform;

            Vector2 targetHorizontalPosition = new Vector2(targetTransform.position.x, 0);
            Vector2 myHorizontalPosition = new Vector2(_enemy.transform.position.x, 0);

            float horizontalDistance = Vector2.Distance(targetHorizontalPosition, myHorizontalPosition);

            if (horizontalDistance < 2)
            {
                return null;
            }
            return targetTransform;
        }

        private Transform GetFirstNodeToFollow(Node[] paths)
        {
            Transform targetPosition = _enemy.GameManager.Player.transform;
            if (paths.Length > 0)
            {
                targetPosition = paths[0].transform;
            }
            return targetPosition;
        }

        private void CalculateDirection(Transform targetPosition)
        {
            if (targetPosition.position.x > _enemy.transform.position.x)
            {
                _direction = new Vector2(1, 0);
            }
            else if (targetPosition.position.x < _enemy.transform.position.x)
            {
                _direction = new Vector2(-1, 0);
            }
        }
    }
}
