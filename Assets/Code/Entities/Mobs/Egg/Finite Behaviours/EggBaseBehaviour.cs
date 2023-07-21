public abstract class EggBaseBehaviour : EnemyBaseBehaviour
{
    protected Egg _egg;
    
    public abstract Egg.Behaviour Behaviour { get; }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _egg = (Egg) enemy;
    }
    
}