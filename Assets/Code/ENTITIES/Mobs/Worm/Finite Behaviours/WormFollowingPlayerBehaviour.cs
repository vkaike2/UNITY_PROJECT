using Calcatz.MeshPathfinding;
using System.Collections;
using System.Linq;
using UnityEngine;

public class WormFollowingPlayerBehaviour : WormFiniteBaseBehaviour
{
    public override Worm.Behaviour Behaviour => Worm.Behaviour.FollowingPlayer;

    private Vector2 _direction = Vector2.zero;

    public override void OnEnterBehaviour()
    {
        _worm.Animator.PlayAnimation(WormAnimatorModel.AnimationName.Move);
        _worm.Pathfinding.StartFindPath(0);
    }

    public override void OnExitBehaviour()
    {
        _worm.Pathfinding.StopPathFinding();
    }

    public override void Update()
    {
        if (!_worm.CanMove)
        {
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            return;
        };

        Node[] paths = _worm.Pathfinding.GetPathResult();
        if (paths == null)
        {
            _worm.ChangeBehaviour(Worm.Behaviour.Patrol);
            return;
        }
        CalculateDirection(paths);

        _worm.RotationalTransform.localScale = new Vector3(_direction.x, 1, 1);
        _rigidbody2D.velocity = new Vector2(_direction.x * _worm.Status.MovementSpeed.Get(), 0);
    }

    private void CalculateDirection(Node[] paths)
    {
        Transform targetPosition = _worm.GameManager.Player.transform;
        if (paths.Length > 0)
        {
            targetPosition = paths[0].transform;
        }

        if (targetPosition.position.x > _worm.transform.position.x)
        {
            _direction = new Vector2(1, 0);
        }
        else if (targetPosition.position.x < _worm.transform.position.x)
        {
            _direction = new Vector2(-1, 0);
        }
    }
}
