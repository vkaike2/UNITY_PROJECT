using Calcatz.MeshPathfinding;
using System;
using static EnemyFollowingBehavior;

public partial class Chicken : Enemy
{
    public class FollowingWorm : ChickenBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.FollowingWorm;

        private ChickenFollowingModel _followingModel;
        private EnemyFollowingBehavior _followingBehavior;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _followingModel = _chicken.FollowingModel;
            _followingBehavior = new EnemyFollowingBehavior(MovementType.Jump, _followingModel);
            _followingBehavior.Start(enemy);
        }

        public override void OnEnterBehaviour()
        {
            AssignEvents();
            _chicken.WormPathfinding.SetTarget(_chicken.AtkWormModel.WormTarget.transform);
            _chicken.WormPathfinding.StartFindPath(Pathfinding.PossibleActions.Vertical);
            _followingBehavior.Pathfinding = _chicken.WormPathfinding;

            _followingBehavior.OnEnterBehaviour();
        }

        public override void OnExitBehaviour()
        {
            _chicken.WormPathfinding.StopPathFinding();
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
                    _chicken.ChickenAnimator.PlayAnimation(ChickenAnimatorModel.AnimationName.Idle);
                    break;
                case EnemyFollowModel.PossibleAnimations.Move:
                    _chicken.ChickenAnimator.PlayAnimation(ChickenAnimatorModel.AnimationName.Move);
                    break;
                case EnemyFollowModel.PossibleAnimations.Air:
                    _chicken.ChickenAnimator.PlayAnimation(ChickenAnimatorModel.AnimationName.Air);
                    break;
                case EnemyFollowModel.PossibleAnimations.Jump:
                    _chicken.ChickenAnimator.PlayAnimation(ChickenAnimatorModel.AnimationName.Jump);
                    break;
            }

        }

        private void OnInteractWithTarget(Target target)
        {
            if (target == null) return;
            if (target.TargeTransform == null) return;

            _chicken.ChangeBehaviour(Chicken.Behaviour.Atk_Worm);
        }

        private void OnTargetUnreachable()
        {
            _chicken.ChangeBehaviour(Behaviour.Patrol);
        }
    }
}
