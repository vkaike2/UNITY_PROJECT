using Calcatz.MeshPathfinding;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ChickenFollowingBehaviour : ChickenFiniteBaseBehaviour
{
    public override Chicken.Behaviour Behaviour => throw new System.NotImplementedException();

    //Events
    protected Action<Target> _onInteractWithTarget;

    //Parent attributes
    protected Pathfinding _pathfinding;
    private ChickenFollowingBehaviourModel _followingPlayerModel;
    private BoxCollider2D _collider;

    //Internal attributes
    protected readonly Target _target = new Target();
    private readonly Direction _direction = new Direction();

    //State helpers
    private bool _isJumping = false;
    private bool _isDownPlatform = false;

    //Constants
    private readonly float DISTANCE_TO_STOP_FOLLOW = 1F;
    private readonly float DEACTIVATE_COLLIDER_DOWN_PLATFORM = 0.5F;
    private readonly Vector2 DEFAULT_JUMP_VELOCITY = new Vector2(5, 15);

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _gameManager = _chicken.GameManager;
        _collider = _chicken.BoxCollider2D;
        _followingPlayerModel = _chicken.FollowingPlayerModel;
    }

    public override void OnEnterBehaviour() { }

    public override void OnExitBehaviour()
    {
        _pathfinding = null;
    }

    public override void Update()
    {
        if (CheckIfIsCloseToTarget()) return;

        if (ManagePathfinding())
        {
            ManageHorizontalMovement();
            ManageJump();
            ManageDownPlatform();
        }

        if (!IsOnTheGround())
        {
            _chicken.Animator.PlayAnimation(ChickenAnimatorModel.AnimationName.Air);
        }
    }

    #region PATH FINDING
    private bool CheckIfIsCloseToTarget()
    {
        if (_pathfinding.Target == null) return false;

        if (Vector2.Distance(_chicken.transform.position, _pathfinding.Target.position) > DISTANCE_TO_STOP_FOLLOW || !IsOnTheGround()) return false;

        _chicken.Animator.PlayAnimation(ChickenAnimatorModel.AnimationName.Idle);
        _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
        _onInteractWithTarget?.Invoke(_target);

        return true;
    }

    private bool ManagePathfinding()
    {
        if (_pathfinding == null) return false;

        Node[] paths = _pathfinding.GetPathResult();

        if (_isJumping)
        {
            return false;
        }

        if (paths == null)
        {
            _chicken.ChangeBehaviour(Chicken.Behaviour.Patrol);
            return false;
        }

        GetTarget(paths);

        CalculateDirection();

        return true;
    }

    private void GetTarget(Node[] paths)
    {
        _target.ClearBooleans();

        Node node = paths.FirstOrDefault();
        if (node == null)
        {
            _target.TargeTransform = _pathfinding.Target.transform;
            _target.ParentNode = null;

            return;
        }

        if (_target.ParentNode != null)
        {
            _target.CheckIfNeedToGoDownPlatform(node);
            _target.CheckIfNeedToJump(node);
        }

        _target.ParentNode = node;
        _target.TargeTransform = node.transform;
    }

    private void CalculateDirection()
    {
        if (_target == null) return;

        Vector2 myPosition = _chicken.transform.position;
        //float myJumpPosition = (_collider.bounds.max.y + _collider.bounds.min.y) / 2;
        //float myBottonPosition = _collider.bounds.min.y;

        if (_isJumping)
        {
            return;
        }

        _direction.Action = Action.Walk;

        if (_target.Position.x > myPosition.x)
        {
            _direction.CurrentDirection = new Vector2(1, 0);
        }
        else if (_target.Position.x < myPosition.x)
        {
            _direction.CurrentDirection = new Vector2(-1, 0);
        }

        FlipPlayer(_direction.CurrentDirection.x > 0);

        if (_target.NeedToJump)
        {
            _direction.Action = Action.Jump;
        }
        else if (_target.NeedToGoDownPlatform && IsOverPlatform())
        {
            _direction.Action = Action.DownPlatform;
        }

    }
    #endregion

    #region HORIZONTAL MOVEMENT
    private void ManageHorizontalMovement()
    {
        if (_direction.Action == Action.Jump) return;

        float horizontalSpeed = _direction.CurrentDirection.x > 0 ? _chicken.Status.MovementSpeed.Get() : -_chicken.Status.MovementSpeed.Get();
        if (_direction.CurrentDirection == Vector2.zero)
        {
            horizontalSpeed = 0;
        }

        if (IsOnTheGround())
        {
            _chicken.Animator.PlayAnimation(ChickenAnimatorModel.AnimationName.Move);
        }

        _rigidbody2D.velocity = new Vector2(horizontalSpeed, _rigidbody2D.velocity.y);
    }
    #endregion

    #region JUMP
    private void ManageJump()
    {
        if (_direction.Action != Action.Jump) return;
        if (_isJumping) return;
        if (!IsOnTheGround()) return;

        _target.JumpTarget = _target;
        _isJumping = true;

        _chicken.StartCoroutine(StartJump());
    }

    private IEnumerator StartJump()
    {
        _chicken.Animator.PlayAnimation(ChickenAnimatorModel.AnimationName.Jump);
        _rigidbody2D.velocity = Vector2.zero;

        yield return new WaitForSeconds(_followingPlayerModel.CdwBeforeJump);

        Vector3 jumpVelocity = CalculateJumpVelocity(_target.TargeTransform);

        if (float.IsNaN(jumpVelocity.x))
        {
            _rigidbody2D.velocity =
                new Vector2(
                    HasIntentionToGoRight(_target.Position) ? DEFAULT_JUMP_VELOCITY.x : -DEFAULT_JUMP_VELOCITY.x,
                    DEFAULT_JUMP_VELOCITY.y);
        }
        else
        {
            _rigidbody2D.velocity = jumpVelocity;
        }

        if (IsCloseToWall())
        {
            _chicken.StartCoroutine(OvercomeWall(jumpVelocity.normalized.x));
        }

        _chicken.StartCoroutine(WaitLeaveGround());
    }

    private IEnumerator OvercomeWall(float direction)
    {
        while (_rigidbody2D.velocity.y > 0)
        {
            _rigidbody2D.velocity = new Vector2(direction * _chicken.Status.MovementSpeed.Get(), _rigidbody2D.velocity.y);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator WaitLeaveGround()
    {
        while (IsOnTheGround())
        {
            yield return new WaitForFixedUpdate();
        }

        _chicken.StartCoroutine(ManageLanding());
    }

    private IEnumerator ManageLanding()
    {
        _followingPlayerModel.pointA = new Vector2(_chicken.transform.position.x, _collider.bounds.min.y);
        _followingPlayerModel.pointB = _target.JumpTarget.Position;
        _followingPlayerModel.draw = true;

        float generalDistance = Vector2.Distance(new Vector2(_chicken.transform.position.x, _collider.bounds.min.y), _target.JumpTarget.Position);

        if (generalDistance < 0.5f)
        {
            _followingPlayerModel.draw = false;
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);

            _chicken.StartCoroutine(WaitHitGround());
            yield break;
        }

        yield return new WaitForFixedUpdate();
        _chicken.StartCoroutine(ManageLanding());
    }

    private IEnumerator WaitHitGround()
    {
        while (!IsOnTheGround())
        {
            yield return new WaitForFixedUpdate();
        }

        _isJumping = false;
    }

    private Vector3 CalculateJumpVelocity(Transform target)
    {
        float initialAngle = 70f;

        Vector3 p = target.position;
        p = new Vector3(p.x, p.y + 0.1f, p.z);

        float gravity = Physics.gravity.magnitude * 3;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(_chicken.transform.position.x, 0, _chicken.transform.position.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);

        // Distance along the y axis between objects
        float yOffset = _chicken.transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;


        return new Vector3(HasIntentionToGoRight(target.position) ? finalVelocity.x : -finalVelocity.x, finalVelocity.y, 0);
    }
    #endregion

    #region DOWN PLATFORM
    private void ManageDownPlatform()
    {
        if (_direction.Action != Action.DownPlatform) return;
        if (_isDownPlatform) return;

        _isDownPlatform = true;
        _chicken.StartCoroutine(DeactivateColliderFor());
    }

    private IEnumerator DeactivateColliderFor()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(DEACTIVATE_COLLIDER_DOWN_PLATFORM);
        _collider.enabled = true;
        _isDownPlatform = false;
    }
    #endregion

    #region CHECK METHODS
    private bool IsCloseToWall()
    {
        Collider2D col = _followingPlayerModel.WallCheck.DrawPhysics2D(_followingPlayerModel.WallLayer, false);
        return col != null;
    }

    private bool IsOverPlatform()
    {
        Collider2D col = _followingPlayerModel.GroundCheck.DrawPhysics2D(_followingPlayerModel.PlatformLayer);
        return col != null;
    }

    private bool IsOnTheGround()
    {
        Collider2D col = _followingPlayerModel.GroundCheck.DrawPhysics2D(_followingPlayerModel.GroundLayer);
        return col != null;
    }

    private void FlipPlayer(bool right)
    {
        _chicken.RotationalTransform.localScale = new Vector3(right ? 1 : -1, 1, 1);
    }

    private bool HasIntentionToGoRight(Vector2 target)
    {
        return target.x > _chicken.transform.position.x;
    }
    #endregion

    protected class Target
    {
        public bool NeedToJump { get; private set; }
        public bool NeedToGoDownPlatform { get; private set; }

        public Node ParentNode { get; set; }
        public Transform TargeTransform { get; set; }
        public Vector2 Position => TargeTransform.position;

        public Target JumpTarget { get; set; }

        public void ClearBooleans()
        {
            NeedToGoDownPlatform = false;
            NeedToJump = false;
        }

        public void CheckIfNeedToJump(Node node)
        {
            NeedToJump = ParentNode.neighbours.Where(e => e.node.GetInstanceID() == node.GetInstanceID() && e.needToJump).Any();
        }

        public void CheckIfNeedToGoDownPlatform(Node node)
        {
            NeedToGoDownPlatform = ParentNode.neighbours.Where(e => e.node.GetInstanceID() == node.GetInstanceID() && e.needToGoDownPlatform).Any();
        }

        public void Log()
        {
            Debug.Log($"position: {Position} \t name: {TargeTransform.name}");
        }
    }

    protected class Direction
    {
        public Vector2 CurrentDirection { get; set; }
        public Action Action { get; set; }

        public void Log()
        {
            Debug.Log($"direction: {CurrentDirection} \t action: {Action}");
        }
    }

    protected enum Action
    {
        Walk,
        Jump,
        DownPlatform
    }
}
