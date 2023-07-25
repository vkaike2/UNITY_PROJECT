﻿using Calcatz.MeshPathfinding;

public partial class Worm : Enemy
{
    public class Patrol : WormBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Patrol;

        private WormPatrolModel _patrolModel = null;
        private EnemyPatrolBehaviour _patrolBehaviour;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _patrolModel = _worm.PatrolModel;
            _patrolBehaviour = new EnemyPatrolBehaviour(EnemyPatrolBehaviour.MovementType.Walk, _patrolModel);

            AddEventListeners();

            _patrolBehaviour.Start(enemy);
        }
        public override void OnEnterBehaviour()
        {
            _patrolBehaviour.OnEnterBehaviour();
            _worm.Pathfinding.StartFindPath(Pathfinding.PossibleActions.Horizontal);
        }

        public override void OnExitBehaviour()
        {
            _patrolBehaviour.OnExitBehaviour();
            _worm.Pathfinding.StopPathFinding();
        }

        public override void Update()
        {
            _patrolBehaviour.Update();

            if (CheckIfPlayerIsReachable())
            {
                _worm.ChangeBehaviour(Worm.Behaviour.FollowingPlayer);
            }
        }

        private bool CheckIfPlayerIsReachable()
        {
            Node[] pathResult = _worm.Pathfinding.GetPathResult();

            return pathResult != null;
        }

        private void AddEventListeners()
        {
            _patrolModel.OnChangeAnimation.AddListener(OnChangeAnimation);
        }

        private void OnChangeAnimation(EnemyPatrolModel.PossibleAnimations possibleAnimations)
        {
            switch (possibleAnimations)
            {
                case EnemyPatrolModel.PossibleAnimations.Idle:
                    _worm.WormAnimator.PlayAnimation(WormAnimatorModel.AnimationName.Idle);
                    break;
                case EnemyPatrolModel.PossibleAnimations.Move:
                    _worm.WormAnimator.PlayAnimation(WormAnimatorModel.AnimationName.Move);
                    break;
            }
        }
    }
}