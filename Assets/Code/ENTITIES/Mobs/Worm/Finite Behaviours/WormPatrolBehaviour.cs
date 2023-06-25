using Calcatz.MeshPathfinding;


public class WormPatrolBehaviour : WormFiniteBaseBehaviour
{
    public override Worm.Behaviour Behaviour => Worm.Behaviour.Patrol;

    private PatrolService _patrolService;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _patrolService = new PatrolService(
            enemy, 
            _worm.PatrolModel,
            () => _worm.Animator.PlayAnimation(WormAnimatorModel.AnimationName.Idle),
            () => _worm.Animator.PlayAnimation(WormAnimatorModel.AnimationName.Move));
    }

    public override void OnEnterBehaviour()
    {
        _patrolService.StartPatrolBehaviour();
        _worm.Pathfinding.StartFindPath(Pathfinding.PossibleActions.Horizontal);
    }

    public override void OnExitBehaviour()
    {
        _patrolService.StopPatrolBehaviour();
        _worm.Pathfinding.StopPathFinding();
    }

    public override void Update()
    {
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
}
