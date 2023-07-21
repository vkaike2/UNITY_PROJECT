using Calcatz.MeshPathfinding;
using System;
using UnityEngine;

public partial class Worm : Enemy
{
    public class FollowingPlayer : WormBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.FollowingPlayer;

        private EnemyFollowingBehavior _followingBehaviour;

        private readonly EnemyFollowEventsModel _followingEvents = new EnemyFollowEventsModel();

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);

            _followingBehaviour = new EnemyFollowingBehavior(EnemyFollowingBehavior.MovementType.Walk, _followingEvents);
            _followingBehaviour.Start(enemy);

            _followingBehaviour.Pathfinding = _worm.Pathfinding;
        }

        public override void OnEnterBehaviour()
        {
            AssignEvents();
            _followingBehaviour.OnEnterBehaviour();
        }

        public override void OnExitBehaviour()
        {
            _followingBehaviour.OnExitBehaviour();
        }

        public override void Update()
        {
            _followingBehaviour.Update();
        }

        private void AssignEvents()
        {
            _followingEvents.OnTargetUnreachable.AddListener(OnTargetUnreachable);
            _followingEvents.OnChangeAnimation.AddListener(OnChangeAnimation);

        }

        private void OnChangeAnimation(EnemyFollowEventsModel.PossibleAnimations posibleAnimation)
        {
            if(posibleAnimation == EnemyFollowEventsModel.PossibleAnimations.Idle)
            {
                _worm.WormAnimator.PlayAnimation(WormAnimatorModel.AnimationName.Idle);
                return;
            }

            if (posibleAnimation == EnemyFollowEventsModel.PossibleAnimations.Move)
            {
                _worm.WormAnimator.PlayAnimation(WormAnimatorModel.AnimationName.Move);
                return;
            }

        }

        private void OnTargetUnreachable()
        {
            _worm.ChangeBehaviour(Behaviour.Patrol);
        }
    }

}
