using Calcatz.MeshPathfinding;
using System.Collections;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;


public partial class EnemyFollowingBehaviour : MonoBehaviour
{
    public class Walk : EnemyBaseBehaviour
    {
        private readonly EnemyFollowingBehavior _parent;
        private Vector2 _direction = Vector2.zero;
        private EnemyFollowEventsModel _followEventsModel;

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
            if (paths == null)
            {
                _followEventsModel.OnTargetUnreachable.Invoke();
                return;
            }
            CalculateDirection(paths);

            _enemy.RotationalTransform.localScale = new Vector3(_direction.x, 1, 1);
            _rigidbody2D.velocity = new Vector2(_direction.x * _enemy.Status.MovementSpeed.Get(), 0);
        }

        private void CalculateDirection(Node[] paths)
        {
            Transform targetPosition = _enemy.GameManager.Player.transform;
            if (paths.Length > 0)
            {
                targetPosition = paths[0].transform;
            }

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
