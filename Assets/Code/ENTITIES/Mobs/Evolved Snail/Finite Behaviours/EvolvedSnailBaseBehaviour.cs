
public abstract class EvolvedSnailBaseBhaviour : EnemyBaseBehaviour
{
    protected EvolvedSnail _evolvedSnail;

    public abstract EvolvedSnail.Behaviour Behaviour { get; }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _evolvedSnail = (EvolvedSnail)enemy;
    }
}