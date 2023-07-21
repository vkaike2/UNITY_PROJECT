using Calcatz.MeshPathfinding;
using UnityEngine;

public partial class Chicken : Enemy
{
    [field: SerializeField]
    public ChickenPatrolModel PatrolModel { get; private set; }

    public class Patrol : ChickenBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Patrol;

        private ChickenPatrolModel _patrolModel;
        private EnemyPatrolBehaviour _patrolBehaviour;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _patrolModel = _chicken.PatrolModel;

            _patrolBehaviour = new EnemyPatrolBehaviour(EnemyPatrolBehaviour.MovementType.Walk, _patrolModel);

            AddEventListeners();

            _patrolBehaviour.Start(enemy);
        }

        public override void OnEnterBehaviour()
        {
            _patrolBehaviour.OnEnterBehaviour();
        }

        public override void OnExitBehaviour()
        {
            _patrolBehaviour.OnExitBehaviour();
            _chicken.PlayerPathfinding.StopPathFinding();
        }

        public override void Update()
        {
            _patrolBehaviour.Update();
            FindFollowingTarget();
        }

        private void FindFollowingTarget()
        {
            if (!_chicken.IsMaxTier() && CheckIfWormIsReachable())
            {
                _chicken.ChangeBehaviour(Chicken.Behaviour.FollowingWorm);
                return;
            }

            if (CheckIfPlayerIsReachable())
            {
                _chicken.ChangeBehaviour(Chicken.Behaviour.FollowingPlayer);
            }
        }

        private bool CheckIfWormIsReachable()
        {
            Worm worm = _chicken.GameManager.GetNearestWorm(_chicken.transform.position);
            if (worm == null) return false;

            _chicken.AtkWormModel.WormTarget = worm;
            _chicken.WormPathfinding.FindPath(Pathfinding.PossibleActions.Vertical, target: worm.transform);

            Node[] pathResult = _chicken.WormPathfinding.GetPathResult();
            return pathResult != null;
        }

        private bool CheckIfPlayerIsReachable()
        {
            Node[] pathResult = _chicken.PlayerPathfinding.FindPath(Pathfinding.PossibleActions.Vertical);

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
                    _chicken.ChickenAnimator.PlayAnimation(ChickenAnimatorModel.AnimationName.Idle);
                    break;
                case EnemyPatrolModel.PossibleAnimations.Move:
                    _chicken.ChickenAnimator.PlayAnimation(ChickenAnimatorModel.AnimationName.Move);
                    break;
            }
        }
    }
}
