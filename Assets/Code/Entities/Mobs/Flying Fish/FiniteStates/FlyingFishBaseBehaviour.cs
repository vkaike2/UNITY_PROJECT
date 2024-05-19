
using UnityEngine;

public abstract class FlyingFishBaseBehaviour : EnemyBaseBehaviour
{

    protected FlyingFish _flyingFish;

    protected FlyingFishIdleModel _idleModel;
    protected FlyingFishWalkModel _walkModel;
    protected FlyingFishAttackModel _attackModel;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _flyingFish = (FlyingFish)enemy;

        _idleModel = _flyingFish.IdleModel;
        _walkModel = _flyingFish.WalkModel;
        _attackModel = _flyingFish.AttackModel;
    }

    public abstract FlyingFish.Behaviour Behaviour { get; }

    protected void SetVelocity(Vector2 velocity)
    {
        UpdateDirection(velocity);

        _rigidbody2D.velocity = velocity;
    }

    protected void UpdateDirection(Vector2 direction)
    {
        _flyingFish.RotationalTransform.localScale = new Vector3(
                        direction.x > 0 ? 1 : -1,
                        1,
                        1);
    }
}