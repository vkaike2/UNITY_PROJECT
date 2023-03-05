using Pathfinding;
using System.Collections;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;


public class ChickenFollowingPlayerBehaviour : ChickenFiniteBaseBehaviour
{
    public override Chicken.Behaviour Behaviour => Chicken.Behaviour.FollowingPlayer;

    private Path _path = null;
    private Vector2 _direction = Vector2.zero;

    private Coroutine _updatePlayerPathCoroutine = null;

    public override void OnEnterBehaviour()
    {
        _updatePlayerPathCoroutine = _chicken.StartCoroutine(UpdatePlayerPath());
    }

    public override void OnExitBehaviour()
    {
        if (_updatePlayerPathCoroutine == null) return;
        _chicken.StopCoroutine(_updatePlayerPathCoroutine);
    }

    public override void Update()
    {
        if (!_chicken.CanMove) return;

        _rigidbody2D.velocity = new Vector2(_direction.x * _chicken.MovementSpeed, _rigidbody2D.velocity.y);
    }

    private IEnumerator UpdatePlayerPath()
    {
        _mySeeker.StartPath(_chicken.transform.position, _gameManager.Player.transform.position, (Path path) =>
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

        if (_path == null || !_mySeeker.CheckIfTargetIsInRageIfYouJump(_path))
        {
            Debug.Log("back to it");
            _chicken.ChangeBehaviour(Chicken.Behaviour.Patrol);
            _direction = Vector2.zero;
            yield return null;
        }


        Vector2 offsetDirection = ((Vector2)_path.vectorPath[1] - _rigidbody2D.position);
        _direction = offsetDirection.Normalized();
        _chicken.RotationalTransform.localScale = new Vector3(_direction.x == 1 ? 1 : -1, 1, 1);

        yield return new WaitForSeconds(_mySeeker.TICK_PATH_CDW);

        _updatePlayerPathCoroutine = _chicken.StartCoroutine(UpdatePlayerPath());
    }
}
