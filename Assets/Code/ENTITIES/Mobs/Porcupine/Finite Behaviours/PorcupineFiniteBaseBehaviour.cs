public abstract class PorcupineFiniteBaseBehaviour : EnemyFiniteBaseBehaviour
{
    protected Porcupine _porcupine;

    public abstract Porcupine.Behaviour Behaviour { get; }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _porcupine = (Porcupine) enemy;
    }
}
