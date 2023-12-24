using Calcatz.MeshPathfinding;
using UnityEngine;

public partial class Chicken : Enemy
{
    [field: SerializeField]
    public ChickenPatrolModel PatrolModel { get; private set; }

    public class Patrol : ChickenBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Patrol;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
        }

        public override void OnEnterBehaviour()
        {
        }

        public override void OnExitBehaviour()
        {
            _chicken.PlayerPathfinding.StopPathFinding();
        }

        public override void Update()
        {
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
            _chicken.WormPathfinding.FindPath(Pathfinding.PossibleActions.Jump, target: worm.transform);

            Node[] pathResult = _chicken.WormPathfinding.GetPathResult();
            return pathResult != null;
        }

        private bool CheckIfPlayerIsReachable()
        {
            Node[] pathResult = _chicken.PlayerPathfinding.FindPath(Pathfinding.PossibleActions.Jump);

            return pathResult != null;
        }
    }
}
