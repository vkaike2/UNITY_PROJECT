using Calcatz.MeshPathfinding;
using UnityEngine;

public class ChickenPatrolBehaviour : ChickenFiniteBaseBehaviour
{
    public override Chicken.Behaviour Behaviour => Chicken.Behaviour.Patrol;

    private PatrolService _patrolService;
    private ChickenPatrolBehaviourModel _patrolModel;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);

        _patrolService = new PatrolService(
            enemy,
            _chicken.PatrolModel,
            () => _chicken.Animator.PlayAnimation(ChickenAnimatorModel.AnimationName.Idle),
            () => _chicken.Animator.PlayAnimation(ChickenAnimatorModel.AnimationName.Move));

        _patrolModel = _chicken.PatrolModel;
    }

    public override void OnEnterBehaviour()
    {
        _patrolService.StartPatrolBehaviour();
    }

    public override void OnExitBehaviour()
    {
        _patrolService.StopPatrolBehaviour();
        _chicken.PlayerPathfinding.StopPathFinding();
    }

    public override void Update()
    {
        FindFollowingTarget();
    }

    private void FindFollowingTarget()
    {
        if (!_chicken.IsMaxTier() && ChefIfWormIsReachable())
        {
            _chicken.ChangeBehaviour(Chicken.Behaviour.FollowingWorm);
            return;
        }

        if (CheckIfPlayerIsReachable())
        {
            _chicken.ChangeBehaviour(Chicken.Behaviour.FollowingPlayer);
        }
    }

    private bool ChefIfWormIsReachable()
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
}
