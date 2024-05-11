public abstract class StarFishBaseBehaviour : EnemyBaseBehaviour
{
    protected StarFish _starFish;

    protected StarFishIdleModel _idleModel;
    protected StarFishAttckModel _attackModel;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _starFish = (StarFish)enemy;

        _idleModel = _starFish.IdleModel;
        _attackModel = _starFish.AttackModel;

    }

    public abstract StarFish.Behaviour Behaviour { get; }
}