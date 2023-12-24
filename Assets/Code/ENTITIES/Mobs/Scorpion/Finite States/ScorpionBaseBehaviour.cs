public abstract class ScorpionBaseBehaviour : EnemyBaseBehaviour
{
    protected Scorpion _scorpion;
    protected ScorpionIdleModel _idleModel;
    protected ScorpionWalkModel _walkModel;
    protected ScorpionAttackModel _attackModel;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _scorpion = (Scorpion)enemy;
        _idleModel = _scorpion.IdleModel;
        _walkModel = _scorpion.WalkModel;
        _attackModel = _scorpion.AttackModel;
    }

    public abstract Scorpion.Behaviour Behaviour { get; }
}