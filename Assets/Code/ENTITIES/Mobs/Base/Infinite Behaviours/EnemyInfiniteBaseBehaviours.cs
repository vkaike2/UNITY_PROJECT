public abstract class EnemyInfiniteBaseBehaviours
{
    protected Hitbox _hitbox;
    protected GameManager _gameManager;

    public virtual void Start(Enemy enemy)
    {
        _gameManager = enemy.GameManager;
        _hitbox = enemy.HitBox;
    }

    public abstract void Update();
}
