using System;
using UnityEngine;
using UnityEngine.Events;

public partial class EvolvedSnail : Enemy
{
    private class FollowingPLayer : EvolvedSnailBaseBhaviour
    {
        public override Behaviour Behaviour => Behaviour.FollowingPlayer;

        private EvolvedSnailFollowingModel _followingModel;
        private EnemyFollowingBehavior _followingBehaviour;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _followingModel = _evolvedSnail.FollowingModel;
            _followingBehaviour = new EnemyFollowingBehavior(EnemyFollowingBehavior.MovementType.Jump, _followingModel);
            _followingBehaviour.Start(enemy);
        }

        public override void OnEnterBehaviour()
        {
            AssignEvents();
            
            _evolvedSnail.PlayerPathfinding.StartFindPath(Calcatz.MeshPathfinding.Pathfinding.PossibleActions.Jump);
            _followingBehaviour.Pathfinding = _evolvedSnail.PlayerPathfinding;

            _followingBehaviour.OnEnterBehaviour();
        }

        public override void OnExitBehaviour()
        {
            _evolvedSnail.PlayerPathfinding.StopPathFinding();
            _followingBehaviour.OnExitBehaviour();

            _followingModel.RightAtkCheck.OnLayerCheckTriggerEnter.RemoveListener(OnPlayerInRange);
            _followingModel.LeftAtkCheck.OnLayerCheckTriggerEnter.RemoveListener(OnPlayerInRange);
        }

        private void OnPlayerInRange(GameObject collidingWith)
        {
            if (!_evolvedSnail.AttackModel.CanAttack) return;

            _evolvedSnail.ChangeBehaviour(Behaviour.Atk_Player);
        }

        public override void Update()
        {
            _followingBehaviour.Update();
        }


        private void AssignEvents()
        {
            _followingModel.OnInteractWithTarget.AddListener(OnInteractWithTarget);
            _followingModel.OnTargetUnreachable.AddListener(OnTargetUnreachable);
            _followingModel.OnChangeAnimation.AddListener(OnChangeAnimation);

            _followingModel.RightAtkCheck.OnLayerCheckTriggerEnter.AddListener(OnPlayerInRange);
            _followingModel.LeftAtkCheck.OnLayerCheckTriggerEnter.AddListener(OnPlayerInRange);
        }

        private void OnChangeAnimation(EnemyFollowEventsModel.PossibleAnimations nextAnimation)
        {
            switch (nextAnimation)
            {
                case EnemyFollowModel.PossibleAnimations.Idle:
                    _evolvedSnail.Animator.PlayAnimation(EvolvedSnailAnimatorModel.AnimationName.Evolved_Snail_Idle);
                    break;
                case EnemyFollowModel.PossibleAnimations.Move:
                    _evolvedSnail.Animator.PlayAnimation(EvolvedSnailAnimatorModel.AnimationName.Evolved_Snail_Run);
                    break;
                case EnemyFollowModel.PossibleAnimations.Air:
                    _evolvedSnail.Animator.PlayAnimation(EvolvedSnailAnimatorModel.AnimationName.Evolved_Snail_Air);
                    break;
                case EnemyFollowModel.PossibleAnimations.Jump:
                    _evolvedSnail.Animator.PlayAnimation(EvolvedSnailAnimatorModel.AnimationName.Evolved_Snail_Jump);
                    break;
            }
        }

        private void OnTargetUnreachable()
        {
            _evolvedSnail.ChangeBehaviour(Behaviour.Idle);
        }

        private void OnInteractWithTarget(EnemyFollowingBehavior.Target target)
        {
        }
    }
}