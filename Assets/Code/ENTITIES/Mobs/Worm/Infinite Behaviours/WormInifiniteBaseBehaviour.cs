using Assets.Code.MANAGER;

public abstract class WormInifiniteBaseBehaviour
{
    protected Hitbox _hitbox;
    protected Worm _worm;
    protected GameManager _gameManager;

    public virtual void Start(Worm worm)
    {
        _worm = worm;
        _gameManager = worm.GameManager;
        _hitbox = worm.HitBox;
    }

    public abstract void Update();
}
