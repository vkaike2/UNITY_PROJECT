public abstract class ChickenBaseBehaviour : EnemyBaseBehaviour
{
    protected Chicken _chicken;

    public abstract Chicken.Behaviour Behaviour { get; }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _chicken = (Chicken)enemy;
    }
}
