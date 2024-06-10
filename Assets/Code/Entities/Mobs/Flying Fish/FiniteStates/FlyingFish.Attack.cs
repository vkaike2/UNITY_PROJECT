using System.Collections;
using UnityEngine;

public partial class FlyingFish : Enemy
{
    private class Attack : FlyingFishBaseBehaviour
    {
        public override Behaviour Behaviour => FlyingFish.Behaviour.Attack;

        private const float ACCEPTABLE_HORIZONTAL_DISTANCE_TO_ATTACK = 0.5f;
        private const float TIME_TO_ROTATE_FISH = 0.3f;
        private bool _isAttacking = false;
        private Vector2 _targetPosition;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _attackModel.IsUnderTheWater = true;
            _attackModel.IsReadyToAttack = false;

            _flyingFish.StartCoroutine(ResetAttackCdw());
        }

        public override void OnEnterBehaviour()
        {
            _isAttacking = false;
            _walkModel.OnStartMoving.AddListener(StartMoving);
            _attackModel.IsReadyToAttack = false;

            TryToFollowPlayer();
        }

        public override void OnExitBehaviour()
        {
            _walkModel.OnStartMoving.RemoveListener(StartMoving);

            _flyingFish.StartCoroutine(ResetAttackCdw());
        }

        public override void Update()
        {
        }

        private void StartMoving()
        {
            if (!_isAttacking)
            {
                _flyingFish.StartCoroutine(ManageHorizontalMovement());
                return;
            }

            _flyingFish.StartCoroutine(JumpTowardsPlayer());
        }

        private void TryToFollowPlayer()
        {
            if (FishCanAttackPlayer())
            {
                StartAttack();
                return;
            }
            _flyingFish.Animator.PlayAnimation(FlyingFishAnimatorModel.AnimationName.Flying_Fish_Walk, true);
        }

        private IEnumerator ResetAttackCdw()
        {
            yield return new WaitForSeconds(_attackModel.CdwToAttack.GetRandom());
            _attackModel.IsReadyToAttack = true;
        }

        private bool FishCanAttackPlayer()
        {
            return GetPlayerHorizonalDistance() <= ACCEPTABLE_HORIZONTAL_DISTANCE_TO_ATTACK;
        }

        private float GetPlayerHorizonalDistance()
        {
            Vector2 playerPosition = _flyingFish.GameManager.Player.transform.position;
            Vector2 playerHorizontalPoisition = new Vector2(playerPosition.x, 0);
            Vector2 myHorizontalPosition = new Vector2(_flyingFish.transform.position.x, 0);

            return Vector2.Distance(playerHorizontalPoisition, myHorizontalPosition);
        }

        private Vector2 GetPlayerDirection()
        {
            Vector2 playerPosition = _flyingFish.GameManager.Player.transform.position;
            return new Vector2(playerPosition.x > _flyingFish.transform.position.x ? 1 : -1, 0);
        }

        public IEnumerator ManageHorizontalMovement()
        {
            Vector2 initalVelocity = GetPlayerDirection() * _walkModel.InitialVelocity;
            SetVelocity(initalVelocity);
            Vector2 currentVelocity;

            bool goingToAttack = false;
            float cdw = 0;
            while (cdw < _walkModel.MovementDuration)
            {
                if (FishCanAttackPlayer())
                {
                    StartAttack();
                    goingToAttack = true;
                    break;
                }

                cdw += Time.deltaTime;

                currentVelocity = new Vector2(
                    Mathf.Lerp((GetPlayerDirection() * _walkModel.InitialVelocity).x, 0, cdw / _walkModel.MovementDuration),
                    0);

                _rigidbody2D.velocity = currentVelocity;

                yield return new WaitForFixedUpdate();
            }

            if (!goingToAttack)
            {
                _rigidbody2D.velocity = Vector2.zero;

                _flyingFish.Animator.PlayAnimation(FlyingFishAnimatorModel.AnimationName.Flying_Fish_Walk, true);
            }
        }

        private void StartAttack()
        {
            _isAttacking = true;

            _rigidbody2D.velocity = Vector2.zero;
            _flyingFish.StartCoroutine(RotateFish());
        }

        private IEnumerator RotateFish()
        {
            float cdw = 0;

            _targetPosition = _flyingFish.GameManager.Player.transform.position;

            while (cdw < TIME_TO_ROTATE_FISH)
            {
                cdw += Time.deltaTime;
                yield return new WaitForFixedUpdate();
                float zAxisRotation = Mathf.Lerp(0, GetVerticalAngle(), cdw / TIME_TO_ROTATE_FISH);
                _flyingFish.RotationalTransform.rotation = Quaternion.Euler(0, 0, zAxisRotation);
            }

            _flyingFish.Animator.PlayAnimation(FlyingFishAnimatorModel.AnimationName.Flying_Fish_Walk, true);
        }

        private float GetVerticalAngle()
        {
            return 90 * _flyingFish.RotationalTransform.localScale.x;
        }

        private IEnumerator JumpTowardsPlayer()
        {
            _rigidbody2D.velocity = new Vector2(0, _attackModel.JumpForce);

            yield return new WaitUntil(() => _targetPosition.y < _flyingFish.transform.position.y);

            _rigidbody2D.gravityScale = _attackModel.FlyingGravity;
            _attackModel.ToggleColliderActivation(true);

            // Rotate back to normal Position
            float cdw = 0;
            while (cdw < TIME_TO_ROTATE_FISH)
            {
                cdw += Time.deltaTime;
                yield return new WaitForFixedUpdate();
                float zAxisRotation = Mathf.Lerp(GetVerticalAngle(), 0, cdw / TIME_TO_ROTATE_FISH);
                _flyingFish.RotationalTransform.rotation = Quaternion.Euler(0, 0, zAxisRotation);
            }
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

            // Flaping on the floor
            yield return new WaitUntil(() => _rigidbody2D.velocity == Vector2.zero);

            _flyingFish.Animator.PlayAnimation(FlyingFishAnimatorModel.AnimationName.Flying_Fish_Floor);

            _flyingFish.StartCoroutine(ManageFloorAction());
        }

        private IEnumerator ManageFloorAction()
        {
            _attackModel.IsUnderTheWater = false;
            yield return new WaitForSeconds(_attackModel.FloorDuration);
            
            float verticalPositionToStop = _flyingFish.WaterSection.GetRandomVerticalPosition();
            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
            _attackModel.ToggleColliderActivation(false);

            yield return new WaitUntil(() => _flyingFish.transform.position.y <= verticalPositionToStop);

            _rigidbody2D.gravityScale = 0;
            _rigidbody2D.velocity = Vector2.zero;
            _flyingFish.transform.position = new Vector3(_flyingFish.transform.position.x, verticalPositionToStop, _flyingFish.transform.position.z);

            _attackModel.IsUnderTheWater = true;
            _flyingFish.ChangeBehaviour(Behaviour.Idle);
        }

    }
}