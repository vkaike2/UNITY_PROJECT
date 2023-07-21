

public partial class EnemyPatrolBehaviour
{
    private readonly MovementType _movement;
    private readonly EnemyPatrolModel _enemyPatrolModel;

    private EnemyBaseBehaviour _behaviour;

    public EnemyPatrolBehaviour(MovementType movement, EnemyPatrolModel enemyPatrolModel)
    {
        _movement = movement;
        _enemyPatrolModel = enemyPatrolModel;
    }

    public void Start(Enemy enemy)
    {
        switch (_movement)
        {
            case MovementType.Walk:
                _behaviour = new Walk(_enemyPatrolModel);
                break;
            case MovementType.Fly:
                break;
        }
        _behaviour.Start(enemy);
    }

    public void OnEnterBehaviour() => _behaviour?.OnEnterBehaviour();
    public void OnExitBehaviour() => _behaviour?.OnExitBehaviour();
    public void Update() => _behaviour?.Update();


    public enum MovementType
    {
        Walk,
        Fly
    }
}
