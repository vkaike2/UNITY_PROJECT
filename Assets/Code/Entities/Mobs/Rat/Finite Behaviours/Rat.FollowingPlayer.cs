using Calcatz.MeshPathfinding;
using System.Collections;
using UnityEngine;
using static EnemyFollowingBehavior;

public partial class Rat : Enemy
{
    private class FollowingPlayer : RatBaseBehaviour
    {
        public override Behaviour Behaviour => Rat.Behaviour.FollowingPlayer;

        private EnemyFollowingBehavior _followingBehavior;
        private RatFollowingModel _followingModel;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _followingModel = _rat.FollowingModel;
            _followingBehavior = new EnemyFollowingBehavior(MovementType.Jump, _followingModel);
            _followingBehavior.Start(enemy);
        }

        public override void OnEnterBehaviour()
        {
            AssignEvents();
            _rat.PlayerPathfinding.StartFindPath(Pathfinding.PossibleActions.Vertical);
            _followingBehavior.Pathfinding = _rat.PlayerPathfinding;

            _followingBehavior.OnEnterBehaviour();
        }

        public override void OnExitBehaviour()
        {
            _rat.PlayerPathfinding.StopPathFinding();
            _followingBehavior.OnExitBehaviour();
        }

        public override void Update()
        {
            _followingBehavior.Update();
        }

        private void AssignEvents()
        {
            _followingModel.OnInteractWithTarget.AddListener(OnInteractWithTarget);
            _followingModel.OnTargetUnreachable.AddListener(OnTargetUnreachable);
            _followingModel.OnChangeAnimation.AddListener(OnChangeAnimation);
        }

        private void OnChangeAnimation(EnemyFollowModel.PossibleAnimations nextAnimation)
        {
            switch (nextAnimation)
            {
                case EnemyFollowModel.PossibleAnimations.Idle:
                    _rat.RatAnimator.PlayAnimation(RatAnimatorModel.AnimationName.Rat_Idle);
                    break;
                case EnemyFollowModel.PossibleAnimations.Move:
                    _rat.RatAnimator.PlayAnimation(RatAnimatorModel.AnimationName.Rat_Moving);
                    break;
                case EnemyFollowModel.PossibleAnimations.Air:
                    _rat.RatAnimator.PlayAnimation(RatAnimatorModel.AnimationName.Rat_Air);
                    break;
                case EnemyFollowModel.PossibleAnimations.Jump:
                    //_rat.RatAnimator.PlayAnimation(RatAnimatorModel.AnimationName.Rat_Jump);
                    break;
            }

        }

        private void OnInteractWithTarget(Target target)
        {
            //if (target == null) return;
            //if (target.TargeTransform == null) return;

            //_ra.ChangeBehaviour(Behaviour.Atk_Player);
        }

        private void OnTargetUnreachable()
        {
            _rat.ChangeBehaviour(Behaviour.Idle);
        }
    }
}
