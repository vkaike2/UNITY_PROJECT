using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public partial class Bat : Enemy
{
    private class Patrol : BatBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Patrol;

        private BatPatrolModel _model;
        private Vector2 _originalPosition;
        private Coroutine _patrolCoroutine;

        private bool _startCheckForPlayerDistance = false;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _originalPosition = enemy.transform.position;
            _model = _bat.PatrolModel;
        }

        public override void OnEnterBehaviour()
        {
            _rigidbody2D.gravityScale = _model.GravityScale;
            ResetVelocity();
            _bat.OnFlapWings.AddListener(OnFlapWings);
            _bat.Animator.PlayAnimation(BatAnimatorModel.AnimationName.Bat_Idle);
            
            _bat.StartCoroutine(WaitForPlayerToBeAdded());
            _patrolCoroutine = _bat.StartCoroutine(ManagePatrol());
        }

        public override void OnExitBehaviour()
        {
            _bat.OnFlapWings.RemoveListener(OnFlapWings);

            _bat.StopCoroutine(_patrolCoroutine);
            ResetVelocity();
            _model.NewPosition = null;
        }

        public override void Update()
        {
            CheckIfPlayerIsInRange();
        }

        private IEnumerator ManagePatrol()
        {
            Vector2 position = GetNextPatrolPosition();

            yield return FlyToNextPosition(position);
            yield return new WaitForSeconds(1f);
            yield return FlyToNextPosition(position, false);

            yield return new WaitForSeconds(_model.CdwBetweenWalks);

            yield return FlyToNextPosition(_originalPosition);
            yield return new WaitForSeconds(1f);
            yield return FlyToNextPosition(_originalPosition, false);

            yield return new WaitForSeconds(_model.CdwBetweenWalks);

            _patrolCoroutine = _bat.StartCoroutine(ManagePatrol());
        }

        private IEnumerator FlyToNextPosition(Vector2 position, bool changeAnimationToFlying = true)
        {
            float distance = Vector2.Distance(_bat.transform.position, position);

            if (changeAnimationToFlying)
            {
                _bat.Animator.PlayAnimation(BatAnimatorModel.AnimationName.Bat_Flying);
            }

            _model.NewPosition = position;

            while (distance > DISTANCE_TO_STOP_FOLLOW)
            {
                distance = Vector2.Distance(_bat.transform.position, position);
                yield return new WaitForFixedUpdate();
                UpdateSpeedToFollowTarget(position);
            }
            _model.NewPosition = null;

            ResetVelocity();

            _bat.Animator.PlayAnimation(BatAnimatorModel.AnimationName.Bat_Idle);
        }

        private Vector2 GetNextPatrolPosition()
        {
            float randomX = Random.Range(-_model.PatrolDistance, _model.PatrolDistance);
            float randomY = Random.Range(-_model.PatrolDistance, _model.PatrolDistance);

            Vector2 newPosition = new Vector2(randomX, randomY);

            RaycastHit2D[] hits = Physics2D.RaycastAll(newPosition, Vector2.zero, 10f, _model.GroundLayer);
            bool hitTheGround = hits != null && hits.Any();

            if (hitTheGround)
            {
                return GetNextPatrolPosition();
            }

            return newPosition;
        }

        private void CheckIfPlayerIsInRange()
        {
            if (!_startCheckForPlayerDistance) return;

            float distance = Vector2.Distance(_gameManager.Player.transform.position, _bat.transform.position);
            if (distance > _model.DistanceToStartFollowPlayer)
            {
                _bat.ChangeBehaviour(Behaviour.FollowingPlayer);
            }
        }

        private IEnumerator WaitForPlayerToBeAdded()
        {
            yield return new WaitUntil(() => _gameManager.Player != null);
            _startCheckForPlayerDistance = true;
        }
    }
}
