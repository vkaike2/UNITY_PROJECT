
public abstract class PorcupineInfiniteBaseBehaviour : EnemyInfiniteBaseBehaviours
{
    protected Porcupine _porcupine;

    public override void Start(Enemy enemy)
    {
        _porcupine = (Porcupine)enemy;
        base.Start(enemy);
    }
}
