
public abstract class WormFiniteBaseBehaviour : EnemyFiniteBaseBehaviour
{
    protected Worm _worm;

    public abstract Worm.Behaviour Behaviour { get; }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _worm = (Worm) enemy;
    }
}
