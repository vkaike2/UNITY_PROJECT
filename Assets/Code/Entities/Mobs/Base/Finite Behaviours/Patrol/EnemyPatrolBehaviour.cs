

public partial class EnemyPatrolBehaviour
{
    private readonly MovementType _movement;
    private readonly EnemyPatrolModel _enemyPatrolModel;
    private readonly EnemyWalkModel _enemyWalkModel;

    private EnemyBaseBehaviour _behaviour;

    public EnemyPatrolBehaviour(MovementType movement, EnemyPatrolModel enemyPatrolModel)
    {
        _movement = movement;
        _enemyPatrolModel = enemyPatrolModel;
    }
    public EnemyPatrolBehaviour(MovementType movement, EnemyWalkModel enemyWalkModel)
    {
        _movement = movement;
        _enemyWalkModel = enemyWalkModel;
    }

    public void Start(Enemy enemy)
    {
        switch (_movement)
        {
            case MovementType.Patrol:
                _behaviour = new Patrol(_enemyPatrolModel);
                break;
            case MovementType.Walk:
                _behaviour = new Walk(_enemyWalkModel);
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
        // It includes walk and Idle
        Patrol,
        // Walk from one side to another
        Walk,
        Fly
    }
}
