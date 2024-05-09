public abstract class SharkBaseBehaviour : EnemyBaseBehaviour
{
    protected Shark _shark;

    protected SharkWalkModel _walkModel;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _shark = (Shark)enemy;

        _walkModel = _shark.WalkModel;
    }

    public abstract Shark.Behaviour Behaviour { get; }
}