using UnityEngine;

public abstract class PistolCrabBaseBehaviour : EnemyBaseBehaviour
{
    public abstract PistolCrab.Behaviour Behaviour { get; }

    protected PistolCrab _pistolCrab;

    protected PistolCrabIdleModel _idleModel;
    protected PistolCrabWalkModel _walkModel;
    protected PistolCrabAttackModel _attackModel;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _pistolCrab = (PistolCrab)enemy;

        _idleModel = _pistolCrab.IdleModel;
        _walkModel = _pistolCrab.WalkModel;
        _attackModel = _pistolCrab.AttackModel;
    }

    protected bool CanAttackPlayer()
    {
        if (!_attackModel.CanAttack) return false;

        return 
            Vector2.Distance(_pistolCrab.GameManager.Player.transform.position, _pistolCrab.transform.position)
            <= _attackModel.DistanceToAttackPlayer;
    }
}