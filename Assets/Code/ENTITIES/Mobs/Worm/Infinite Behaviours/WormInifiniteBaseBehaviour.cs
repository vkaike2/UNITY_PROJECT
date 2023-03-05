
public abstract class WormInifiniteBaseBehaviour : EnemyInfiniteBaseBehaviours
{
    protected Worm _worm;

    public override void Start(Enemy enemy)
    {
        _worm = (Worm) enemy;
        base.Start(enemy);
    }
}
