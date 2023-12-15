﻿using Calcatz.MeshPathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnemyFollowingBehavior;

public partial class EnemyFollowingBehaviour
{
    public class Jump : EnemyBaseBehaviour
    {
        private readonly EnemyFollowingBehavior _parent;
        private readonly EnemyFollowModel _model;

        //Internal attributes
        protected readonly Target _target = new Target();
        private readonly Direction _direction = new Direction();

        //State helpers
        private bool _isJumping = false;
        private bool _isDownPlatform = false;

        //Constants
        private readonly float DEACTIVATE_COLLIDER_DOWN_PLATFORM = 0.2F;
        private readonly Vector2 DEFAULT_JUMP_VELOCITY = new Vector2(5, 15);
        private readonly float MAXIMUM_DISTANCE_FROM_TARGET_WHEN_JUMP = 0.5f;
        private readonly float MAX_TIME_TO_FORGTET_ABOUT_TARGET_WHEN_JUMP = 0.5f;

        public Jump(EnemyFollowingBehavior parent)
        {
            _parent = parent;
            _model = parent.EnemyFollowingModel;
        }

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
        }

        public override void OnEnterBehaviour()
        {
        }

        public override void OnExitBehaviour()
        {
            _model.ResetEvents();
            _parent.Pathfinding = null;
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
                _model.OnChangeAnimation.Invoke(EnemyFollowModel.PossibleAnimations.Air);
            }
        }


        #region PATH FINDING
        private bool CheckIfIsCloseToTarget()
        {
            if (_parent.Pathfinding.Target == null) return false;

            if (Vector2.Distance(_enemy.transform.position, _parent.Pathfinding.Target.position) > _model.DistanceToStopFollow || !IsOnTheGround()) return false;

            _model.OnChangeAnimation.Invoke(EnemyFollowModel.PossibleAnimations.Idle);
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);

            _model.OnInteractWithTarget.Invoke(_target);

            return true;
        }

        //Need to be a coroutine
        private bool ManagePathfinding()
        {
            if (_parent.Pathfinding == null) return false;

            Node[] paths = _parent.Pathfinding.GetPathResult();

            if (_isJumping)
            {
                return false;
            }

            if (paths == null)
            {
                _model.OnTargetUnreachable.Invoke();
                return false;
            }

            GetTarget(paths);

            CalculateDirection();

            return true;
        }

        private void GetTarget(Node[] paths)
        {
            _target.ClearBooleans();

            _target.ParentNode = paths.FirstOrDefault();
            Node node = null;

            if (paths?.Count() > 1)
            {
                node = paths[1];
            }


            //Debug.Log($"{node.gameObject.name} - {Vector2.Distance(node.transform.position, _enemy.transform.position)}");
            
            
            if (node == null)
            {
                _target.TargeTransform = _parent.Pathfinding.Target.transform;
                _target.ParentNode = null;

                return;
            }

            if (_target.ParentNode != null)
            {
                _target.CheckIfNeedToGoDownPlatform(node);
                _target.CheckIfNeedToJump(node);
            }

            //_target.ParentNode = node;
            _target.TargeTransform = node.transform;
        }

        private bool IgnoreFirstNodeIsTooClose(Node node)
        {
            return Vector2.Distance(node.transform.position, _enemy.transform.position) > 2f;
        }

        private void CalculateDirection()
        {
            if (_target == null) return;

            Vector2 myPosition = _enemy.transform.position;

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

            float horizontalSpeed = _direction.CurrentDirection.x > 0 ? _enemy.Status.MovementSpeed.Get() : -_enemy.Status.MovementSpeed.Get();
            if (_direction.CurrentDirection == Vector2.zero)
            {
                horizontalSpeed = 0;
            }

            if (IsOnTheGround())
            {
                _model.OnChangeAnimation.Invoke(EnemyFollowModel.PossibleAnimations.Move);
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

            _enemy.StartCoroutine(StartJump());
        }

        private IEnumerator StartJump()
        {
            _model.OnChangeAnimation.Invoke(EnemyFollowModel.PossibleAnimations.Jump);
            _rigidbody2D.velocity = Vector2.zero;

            yield return new WaitForSeconds(_model.CdwBeforeJump);

            Vector2 jumpVelocity = MovementUtils.CalculateJumpVelocity(_target.TargeTransform.position, _enemy.transform.position);

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

            //if (IsCloseToWa  //{
            //    _enemy.StartCoroutine(OvercomeWall(jumpVelocity.normalized.x));
            //}ll())


            _enemy.StartCoroutine(WaitLeaveGround());
        }

        private IEnumerator OvercomeWall(float direction)
        {
            while (_rigidbody2D.velocity.y > 0)
            {
                _rigidbody2D.velocity = new Vector2(direction * _enemy.Status.MovementSpeed.Get(), _rigidbody2D.velocity.y);
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator WaitLeaveGround()
        {
            while (IsOnTheGround())
            {
                yield return new WaitForFixedUpdate();
            }

            _enemy.StartCoroutine(ManageLanding());
        }

        private IEnumerator ManageLanding()
        {
            _model.pointA = new Vector2(_enemy.transform.position.x, _model.MainCollider.bounds.min.y);
            _model.pointB = _target.JumpTarget.Position;
            _model.draw = true;

            float generalDistance =
                Vector2.Distance(
                    new Vector2(
                        _model.MainCollider.transform.position.x, _model.MainCollider.bounds.min.y)
                     , _target.JumpTarget.Position);


            float cdw = 0;

            while (generalDistance >= MAXIMUM_DISTANCE_FROM_TARGET_WHEN_JUMP && cdw <= MAX_TIME_TO_FORGTET_ABOUT_TARGET_WHEN_JUMP)
            {
                yield return new WaitForFixedUpdate();
                cdw += Time.deltaTime;
                generalDistance =
                Vector2.Distance(
                    new Vector2(
                        _model.MainCollider.transform.position.x, _model.MainCollider.bounds.min.y)
                     , _target.JumpTarget.Position);
            }


            _model.draw = false;
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);

            _enemy.StartCoroutine(WaitHitGround());
            yield break;
        }

        private IEnumerator WaitHitGround()
        {
            while (!IsOnTheGround())
            {
                yield return new WaitForFixedUpdate();
            }
            _isJumping = false;
        }

        //private Vector3 CalculateJumpVelocity(Transform target)
        //{
        //    float initialAngle = 70f;

        //    Vector3 p = target.position;
        //    p = new Vector3(p.x, p.y + 0.1f, p.z);

        //    float gravity = Physics.gravity.magnitude * 3;
        //    // Selected angle in radians
        //    float angle = initialAngle * Mathf.Deg2Rad;

        //    // Positions of this object and the target on the same plane
        //    Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        //    Vector3 planarPostion = new Vector3(_enemy.transform.position.x, 0, _enemy.transform.position.z);

        //    // Planar distance between objects
        //    float distance = Vector3.Distance(planarTarget, planarPostion);

        //    // Distance along the y axis between objects
        //    float yOffset = _enemy.transform.position.y - p.y;
        //    Debug.Log(yOffset);

        //    float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        //    Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        //    // Rotate our velocity to match the direction between the two objects
        //    float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion);
        //    Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;


        //    return new Vector3(HasIntentionToGoRight(target.position) ? finalVelocity.x : -finalVelocity.x, finalVelocity.y, 0);
        //}


        #endregion

        #region DOWN PLATFORM
        private void ManageDownPlatform()
        {
            if (_direction.Action != Action.DownPlatform) return;
            if (_isDownPlatform) return;

            _isDownPlatform = true;
            _enemy.StartCoroutine(DeactivateColliderFor());
        }

        private IEnumerator DeactivateColliderFor()
        {
            _model.MainCollider.enabled = false;
            yield return new WaitForSeconds(DEACTIVATE_COLLIDER_DOWN_PLATFORM);
            _model.MainCollider.enabled = true;
            _isDownPlatform = false;
        }
        #endregion

        #region CHECK METHODS
        private bool IsOverPlatform()
        {
            Collider2D col = _model.GroundCheck.DrawPhysics2D(_model.PlatformLayer);
            return col != null;
        }

        private bool IsOnTheGround()
        {
            Collider2D col = _model.GroundCheck.DrawPhysics2D(_model.GroundLayer);
            return col != null;
        }

        private bool IsOnAPlatform()
        {
            Collider2D col = _model.GroundCheck.DrawPhysics2D(_model.GroundLayer);
            if (col == null) return false;
            if (col.gameObject.GetComponent<OneWayPlatform>() != null) return true;
            return false;
        }

        private void FlipPlayer(bool right)
        {
            _enemy.RotationalTransform.localScale = new Vector3(right ? 1 : -1, 1, 1);
        }

        private bool HasIntentionToGoRight(Vector2 target)
        {
            return target.x > _enemy.transform.position.x;
        }
        #endregion

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
}
