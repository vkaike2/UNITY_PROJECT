using Pathfinding;
using System.Collections;
using UnityEngine;

public class WormFollowingPlayerBehaviour : WormFiniteBaseBehaviour
{
    public override Worm.Behaviour Behaviour => Worm.Behaviour.FollowingPlayer;

    private Path _path = null;
    private Vector2 _direction = Vector2.zero;

    private Coroutine _updatePlayerPathCoroutine = null;

    public override void OnEnterBehaviour()
    {
        _updatePlayerPathCoroutine = _worm.StartCoroutine(UpdatePlayerPath());
    }

    public override void OnExitBehaviour()
    {
        if (_updatePlayerPathCoroutine == null) return;

        _worm.StopCoroutine(_updatePlayerPathCoroutine);
    }

    public override void Update()
    {
        if (!_worm.CanMove) return;

        _rigidbody2D.velocity = new Vector2(_direction.x * _worm.MovementSpeed, _rigidbody2D.velocity.y);
    }

    private IEnumerator UpdatePlayerPath()
    {
        _mySeeker.StartPath(_worm.transform.position, _gameManager.Player.transform.position, (Path path) =>
        {
            if (path.error)
            {
                _path = null;
                return;
            }
            else
            {
                _path = path;
            }
        });

        while (!_mySeeker.IsDone())
        {
            yield return new WaitForFixedUpdate();
        }

        if (_path == null || !_mySeeker.CheckIfTargetIsInRangeIfYouWalk(_path))
        {
            _worm.ChangeBehaviour(Worm.Behaviour.Patrol);
            _direction = Vector2.zero;
            yield return null;
        }


        Vector2 offsetDirection = ((Vector2)_path.vectorPath[1] - _rigidbody2D.position);
        _direction = offsetDirection.Normalized();
        _worm.RotationalTransform.localScale = new Vector3(_direction.x == 1 ? 1 : -1, 1, 1);

        yield return new WaitForSeconds(_mySeeker.TICK_PATH_CDW);

        _updatePlayerPathCoroutine = _worm.StartCoroutine(UpdatePlayerPath());
    }

}
