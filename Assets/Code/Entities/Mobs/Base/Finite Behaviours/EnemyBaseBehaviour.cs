using UnityEngine;

public abstract class EnemyBaseBehaviour
{
    protected Rigidbody2D _rigidbody2D;
    protected GameManager _gameManager;
    protected Enemy _enemy;

    public virtual void Start(Enemy enemy)
    {
        _enemy = enemy;
        _gameManager = enemy.GameManager;
        _rigidbody2D = enemy.GetComponent<Rigidbody2D>();
    }

    public abstract void Update();

    public abstract void OnEnterBehaviour();
    public abstract void OnExitBehaviour();
}
