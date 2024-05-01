using UnityEngine;

public abstract class ArmadilloBaseBehaviour
{
    public abstract Armadillo.Behaviour Behaviour { get; }

    protected Armadillo _armadillo;
    protected Rigidbody2D _rigidBody2D;

    protected ArmadilloAnimatorModel _mainAnimator;

    protected ArmadilloRunningTowardsWallModel _runningTowardsWallModel;
    protected ArmadilloShockwaveModel _shockwaveModel;
    protected ArmadilloIntoBallModel _intoBallModel;
    protected ArmadilloIdleModel _idleModel;

    protected Direction _currentDirection;

    public virtual void Start(Armadillo armadillo)
    {
        _armadillo = armadillo;
        _mainAnimator = _armadillo.MainAnimator;
        _runningTowardsWallModel = _armadillo.RunningTowardsWallModel;
        _shockwaveModel = _armadillo.ShockwaveModel;
        _intoBallModel = _armadillo.IntoBallModel;
        _idleModel = _armadillo.IdleModel;

        _rigidBody2D = armadillo.GetComponent<Rigidbody2D>();
        RotateArmadillo(Direction.Right);
    }

    public abstract void Update();

    public abstract void OnEnterBehaviour();
    public abstract void OnExitBehaviour();

    protected void RotateArmadillo(Direction direction)
    {
        _currentDirection = direction;
        
        _armadillo.RotationalTransform.localScale = new Vector3(
            direction == Direction.Right ? 1 : -1, 
            1, 
            1);
    }


    protected enum Direction
    {
        Left, Right
    }
}