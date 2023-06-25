public abstract class EggFiniteBaseBehaviour : EnemyFiniteBaseBehaviour
{

    protected Egg _egg;
    
    public abstract Egg.Behaviour Behaviour { get; }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _egg = (Egg)enemy;
    }
    
}