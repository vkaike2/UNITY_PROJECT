using System.Collections;
using System.Linq;
using UnityEngine;

public partial class Snail : Enemy
{
    private class Walking : SnailBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Walking;

        private SnailWalkingModel _model;
        private Rigidbody2D _rigidBody2d;

        private Vector2 _movementSpeed;
        private float _initialGravity;

        private const float PADDING_TURINING_AMOUNT = 0.5f;

        private bool _canMove;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);

            _model = _snail.WalkingModel;
            _rigidBody2d = _snail.RigidBody2D;

            _initialGravity = _rigidBody2d.gravityScale;
        }

        public override void OnEnterBehaviour()
        {
            _snail.StartCoroutine(WaitUntilTouchTheGround());
        }

        public override void OnExitBehaviour()
        {
            _rigidBody2d.gravityScale = _initialGravity;
        }

        public override void Update()
        {
            if (!_canMove) return;
            _rigidBody2d.velocity = _snail.CanMove ? _movementSpeed : new Vector2(_movementSpeed.x > 0 ? 0.1f : 0, _movementSpeed.y > 0 ? 0.1f : 0);
        }

        private void GetInitialVelocity()
        {
            _movementSpeed = new Vector2(_snail.Status.MovementSpeed.Get(), 0);
        }

        private IEnumerator ManageMovement()
        {
            _canMove = true;
            
            yield return new WaitUntil(() => _snail.IsCollidingWithGround() && _rigidBody2d.velocity != Vector2.zero);

            bool needToChangeDirection = !_snail.IsCollidingWithGround() || _rigidBody2d.velocity == Vector2.zero;

            while (!needToChangeDirection)
            {
                needToChangeDirection = !_snail.IsCollidingWithGround() || (_rigidBody2d.velocity == Vector2.zero && _snail.CanMove);
                yield return new WaitForFixedUpdate();
            }

            _canMove = false;
            
            _snail.StartCoroutine(ChangeObjectDirection(_snail.IsCollidingWithGround()));
        }

        private IEnumerator ChangeObjectDirection(bool isComplexChange)
        {
            Direction direction = CalculateDirection();
            RotateSprite(direction);

            _rigidbody2D.velocity = CalculateMovementSpeedUntilTouchGround(direction, isComplexChange);
            yield return new WaitUntil(() => _snail.IsCollidingWithGround());

            CalculateMovementSpeed(direction);

            _snail.StartCoroutine(ManageMovement());
        }

        private void CalculateMovementSpeed(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left_UpsideDow:
                    _movementSpeed = new Vector2(-_snail.Status.MovementSpeed.Get(), 0);
                    break;
                case Direction.Right:
                    _movementSpeed = new Vector2(_snail.Status.MovementSpeed.Get(), 0);
                    break;
                case Direction.Up:
                    _movementSpeed = new Vector2(0, _snail.Status.MovementSpeed.Get());
                    break;
                case Direction.Down:
                    _movementSpeed = new Vector2(0, -_snail.Status.MovementSpeed.Get());
                    break;
                default:
                    break;
            }

            return;
        }

        private Vector2 CalculateMovementSpeedUntilTouchGround(Direction direction, bool isComplexChange)
        {

            switch (direction)
            {
                case Direction.Left_UpsideDow:
                    return new Vector2(-_snail.Status.MovementSpeed.Get(), _initialGravity);
                case Direction.Right:
                    return new Vector2(_snail.Status.MovementSpeed.Get(), -_initialGravity);
                case Direction.Up:
                    return new Vector2(_initialGravity, _snail.Status.MovementSpeed.Get());
                case Direction.Down:
                    return new Vector2(-_initialGravity, -_snail.Status.MovementSpeed.Get());
                default:
                    break;
            }


            return Vector2.zero;
        }

        private Direction CalculateDirection()
        {
            bool hitUp = IsHitting(_model.UpCheck);
            bool hitDown = IsHitting(_model.DownCheck);
            bool hitRight = IsHitting(_model.RightCheck);
            bool hitLeft = IsHitting(_model.LeftCheck);

            // calculate basic moves
            bool shouldGoUp = hitDown && hitRight;
            if (shouldGoUp) return Direction.Up;

            bool shouldGoLeft = hitUp && hitRight;
            if (shouldGoLeft) return Direction.Left_UpsideDow;

            bool shouldGoDown = hitUp && hitLeft;
            if (shouldGoDown) return Direction.Down;

            bool shouldGoRight = hitLeft && hitDown;
            if (shouldGoRight) return Direction.Right;

            // calculate complex moves
            bool upRightHit = IsHitting(_model.UpRightCheck);
            bool upLeftHit = IsHitting(_model.UpLeftCheck);
            bool downRightHit = IsHitting(_model.DownRightCheck);
            bool downLeftHit = IsHitting(_model.DownLeftCheck);

            shouldGoUp = upRightHit;
            if (shouldGoUp) return Direction.Up;

            shouldGoDown = downLeftHit;
            if (shouldGoDown) return Direction.Down;

            shouldGoRight = downRightHit;
            if (shouldGoRight) return Direction.Right;

            shouldGoLeft = upLeftHit;
            if (shouldGoLeft) return Direction.Left_UpsideDow;


            throw new System.Exception("Hey dude The snail can't move into this direction");
        }

        private IEnumerator WaitUntilTouchTheGround()
        {
            yield return new WaitUntil(() => _snail.IsCollidingWithGround());

            _snail.Animator.PlayAnimation(SnailAnimatorModel.AnimationName.Snail_Walk);

            _rigidBody2d.gravityScale = 0;
            GetInitialVelocity();
            _snail.StartCoroutine(ManageMovement());
        }

        private bool IsHitting(Transform checkTransform)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(checkTransform.position, Vector2.zero, 10f, _model.LayerToCheck);
            return hits != null && hits.Any();
        }

        private void RotateSprite(Direction direction)
        {
            var horizontalPos = _snail.transform.position.x;
            var verticalPos = _snail.transform.position.y;
            switch (direction)
            {
                case Direction.Left_UpsideDow:

                    horizontalPos -= PADDING_TURINING_AMOUNT;
                    _snail.transform.position = new Vector2(horizontalPos, verticalPos);

                    _snail.RotationalTransform.localScale = new Vector3(-1, -1, 1);
                    _snail.RotationalTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    _snail.GroundCheck.InvertValues(false);

                    break;
                case Direction.Right:

                    horizontalPos += PADDING_TURINING_AMOUNT;
                    _snail.transform.position = new Vector2(horizontalPos, verticalPos);

                    _snail.RotationalTransform.localScale = new Vector3(1, 1, 1);
                    _snail.RotationalTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    _snail.GroundCheck.InvertValues(false);
                    break;
                case Direction.Up:
                    verticalPos += PADDING_TURINING_AMOUNT;
                    _snail.transform.position = new Vector2(horizontalPos, verticalPos);

                    _snail.RotationalTransform.localScale = new Vector3(1, 1, 1);
                    _snail.RotationalTransform.rotation = Quaternion.Euler(0f, 0f, 90);
                    _snail.GroundCheck.InvertValues(true);
                    break;
                case Direction.Down:
                    verticalPos -= PADDING_TURINING_AMOUNT;
                    _snail.transform.position = new Vector2(horizontalPos, verticalPos);

                    _snail.RotationalTransform.localScale = new Vector3(1, 1, 1);
                    _snail.RotationalTransform.rotation = Quaternion.Euler(0f, 0f, -90);
                    _snail.GroundCheck.InvertValues(true);
                    break;
            }
        }

        private enum Direction
        {
            Left_UpsideDow,
            Right,
            Up,
            Down
        }
    }
}
