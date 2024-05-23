public abstract class SeagullBaseBehaviour : EnemyBaseBehaviour
{
    public abstract Seagull.Behaviour Behaviour { get; }

    protected Seagull _seagull;
    protected SeagullFlyModel _flyModel;
    protected SeagullGroundModel _groundModel;  

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _seagull = (Seagull)enemy;

        _flyModel = _seagull.FlyModel;
        _groundModel = _seagull.GroundModel;
    }
}