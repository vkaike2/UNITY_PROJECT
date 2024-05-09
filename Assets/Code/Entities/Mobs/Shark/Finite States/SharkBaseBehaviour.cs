public abstract class SharkBaseBehaviour : EnemyBaseBehaviour
{
    protected Shark _shark;

    protected SharkWalkModel _walkModel;
    protected SharkAttackModel _attackModel;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _shark = (Shark)enemy;

        _walkModel = _shark.WalkModel;
        _attackModel = _shark.AttackModel;
    }

    public abstract Shark.Behaviour Behaviour { get; }
}