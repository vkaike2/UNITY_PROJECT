using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BatBaseBehaviour : EnemyBaseBehaviour
{
    public abstract Bat.Behaviour Behaviour { get; }

    protected const float DISTANCE_TO_STOP_FOLLOW = 0.5f;
    protected Bat _bat;
    protected float _horizontalSpeed;
    protected float _verticalSpeed;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _bat = (Bat)enemy;
    }

    protected void OnFlapWings()
    {
        _rigidbody2D.velocity = new Vector2(_horizontalSpeed, _verticalSpeed);
    }

    protected void ResetVelocity()
    {
        _horizontalSpeed = 0f;
        _verticalSpeed = _bat.PatrolModel.VerticalSpeed;
    }

    protected void UpdateSpeedToFollowTarget(Vector2 target)
    {
        ResetVelocity();
        if (target.x > _bat.transform.position.x)
        {
            _bat.RotationalTransform.localScale = new Vector3(1, 1, 1);
            _horizontalSpeed = _bat.Status.MovementSpeed.Get();
        }
        // left
        else
        {
            _bat.RotationalTransform.localScale = new Vector3(-1, 1, 1);
            _horizontalSpeed = -_bat.Status.MovementSpeed.Get();
        }

        // up
        if (target.y > _bat.transform.position.y)
        {
            _verticalSpeed += _bat.Status.MovementSpeed.Get() / 2;
        }
        // down
        else
        {
            _verticalSpeed -= _bat.Status.MovementSpeed.Get() / 2;
        }
    }
}
