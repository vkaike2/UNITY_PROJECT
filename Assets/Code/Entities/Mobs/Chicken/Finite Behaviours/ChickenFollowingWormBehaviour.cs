using Calcatz.MeshPathfinding;


public class ChickenFollowingWormBehaviour : ChickenFollowingBehaviour
{
    public override Chicken.Behaviour Behaviour => Chicken.Behaviour.FollowingWorm;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);

        _onInteractWithTarget = (e) => InteractWithTarget(e);
    }

    public override void OnEnterBehaviour()
    {
        _chicken.WormPathfinding.SetTarget(_chicken.AtkWormModel.WormTarget.transform);
        _chicken.WormPathfinding.StartFindPath(Pathfinding.PossibleActions.Vertical);

        _pathfinding = _chicken.WormPathfinding;
    }

    public override void OnExitBehaviour()
    {
        _chicken.WormPathfinding.StopPathFinding();
        base.OnExitBehaviour();
    }

    private void InteractWithTarget(Target target)
    {
        if (target == null) return;
        if (target.TargeTransform == null) return;

        _chicken.ChangeBehaviour(Chicken.Behaviour.Atk_Worm);
    }
}
