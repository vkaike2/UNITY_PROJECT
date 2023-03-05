public abstract class ChickenInfiniteBaseBehaviour : EnemyInfiniteBaseBehaviours
{
    protected Chicken _chicken;

    public override void Start(Enemy enemy)
    {
        _chicken = (Chicken)enemy;
        base.Start(enemy);
    }
}
