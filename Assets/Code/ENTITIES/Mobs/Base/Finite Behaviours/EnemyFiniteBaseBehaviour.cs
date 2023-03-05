using UnityEngine;

public abstract class EnemyFiniteBaseBehaviour
{
    protected Rigidbody2D _rigidbody2D;
    protected MySeeker _mySeeker;
    protected GameManager _gameManager;
    protected Enemy _enemy;


    public virtual void Start(Enemy enemy)
    {
        _enemy = enemy;
        _gameManager = enemy.GameManager;
        _mySeeker = enemy.GetComponent<MySeeker>();
        _rigidbody2D = enemy.GetComponent<Rigidbody2D>();
    }

    public abstract void Update();

    public abstract void OnEnterBehaviour();
    public abstract void OnExitBehaviour();
}
