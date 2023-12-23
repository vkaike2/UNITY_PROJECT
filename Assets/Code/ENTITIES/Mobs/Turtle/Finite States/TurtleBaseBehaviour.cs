public abstract class TurtleBaseBehaviour : EnemyBaseBehaviour
{
    protected Turtle _turtle;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _turtle = (Turtle) enemy;
    }

    public abstract Turtle.Behaviour Behaviour { get; }

}