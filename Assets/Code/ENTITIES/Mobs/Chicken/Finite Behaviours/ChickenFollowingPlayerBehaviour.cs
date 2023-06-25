using Calcatz.MeshPathfinding;

public class ChickenFollowingPlayerBehaviour : ChickenFollowingBehaviour
{
    public override Chicken.Behaviour Behaviour => Chicken.Behaviour.FollowingPlayer;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);

        _onInteractWithTarget = (e) => InteractWithTarget(e);
    }

    public override void OnEnterBehaviour()
    {
        _chicken.PlayerPathfinding.StartFindPath(Pathfinding.PossibleActions.Vertical);
        _pathfinding = _chicken.PlayerPathfinding;
    }

    public override void OnExitBehaviour()
    {
        _chicken.WormPathfinding.StopPathFinding();
        base.OnExitBehaviour();
    }

    private void InteractWithTarget(Target target)
    {
        if (target == null) return;
        if(target.TargeTransform == null) return;

        Player player = _chicken.gameObject.GetComponent<Player>();

        _chicken.ChangeBehaviour(Chicken.Behaviour.Atk_Player);
    }

}
