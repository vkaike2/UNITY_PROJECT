using System.Collections;
using UnityEngine;

public partial class Armadillo : MonoBehaviour
{
    private class RunTowardsWall : ArmadilloBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.RunTowardsWall;

        private Action _action;
        private Vector2 _velocity;

        public override void Start(Armadillo armadillo)
        {
            base.Start(armadillo);
        }

        public override void OnEnterBehaviour()
        {
            _runningTowardsWallModel.WillHitTheWallCheck.OnLayerCheckTriggerEnter.AddListener(WillCollideWithWall);
            _runningTowardsWallModel.AlreadyHitTheWallCheck.OnLayerCheckTriggerEnter.AddListener(HitTheWall);

            StartRunningTowardsWall();
        }

        public override void OnExitBehaviour()
        {
            _runningTowardsWallModel.WillHitTheWallCheck.OnLayerCheckTriggerEnter.RemoveListener(WillCollideWithWall);
            _runningTowardsWallModel.AlreadyHitTheWallCheck.OnLayerCheckTriggerEnter.RemoveListener(HitTheWall);

            _armadillo.Colliders.ActivateCollider(MyColliders.ColliderType.Main);
        }

        public override void Update()
        {
            if (_action != Action.Running) return;
            _rigidBody2D.velocity = _velocity;
        }

        private Direction CalculateDirection()
        {
            var player = _armadillo.GameManager.Player;

            return _armadillo.transform.position.x > player.transform.position.x ? Direction.Left : Direction.Right;
        }

        private void HitTheWall(GameObject collidingWith)
        {
            _armadillo.CameraEvents.OnScreenShakeEvent.Invoke(_armadillo.gameObject);

            _armadillo.StartCoroutine(ManageStunThenChangeToIdle());
        }

        private void WillCollideWithWall(GameObject collidingWith)
        {
            _action = Action.HittingWall;
            _armadillo.MainAnimator.PlayAnimation(ArmadilloAnimatorModel.AnimationName.HIT_WALL);

            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, _rigidBody2D.velocity.y + _runningTowardsWallModel.JumpForce);

            _armadillo.Colliders.ActivateCollider(MyColliders.ColliderType.HitWall);
        }

        private void StartRunningTowardsWall()
        {
            Direction runningDirection = CalculateDirection();
            _armadillo.MainAnimator.PlayAnimation(ArmadilloAnimatorModel.AnimationName.RUN);

            _velocity = new Vector2(runningDirection == Direction.Left ? -1 : 1, 0) * _runningTowardsWallModel.Speed;
            RotateArmadillo(runningDirection);

            _action = Action.Running;
        }

        private IEnumerator ManageStunThenChangeToIdle()
        {
            yield return new WaitUntil(() => _rigidBody2D.velocity == Vector2.zero);
            _armadillo.MainAnimator.PlayAnimation(ArmadilloAnimatorModel.AnimationName.STUNNED);

            yield return new WaitForSeconds(_runningTowardsWallModel.StunDuration);
            var newDirection = _currentDirection == Direction.Left ? Direction.Right : Direction.Left;
            RotateArmadillo(_currentDirection == Direction.Left ? Direction.Right : Direction.Left);
            _armadillo.ChangeBehaviour(Behaviour.Idle);
        }

        public enum Action
        {
            None,
            Running,
            HittingWall,
            Stunned
        }

    }
}