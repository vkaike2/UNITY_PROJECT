using Calcatz.MeshPathfinding;
using System.Linq;
using UnityEngine;
using static EnemyFollowingBehaviour;

public partial class EnemyFollowingBehavior
{

    public Pathfinding Pathfinding { get; set; }


    //used for Jump
    public EnemyFollowModel EnemyFollowingModel { get; private set; }
    //used for Walk only
    public EnemyFollowEventsModel EnemyFollowEventsModel { get; private set; }

    private readonly MovementType _movement;

    private EnemyBaseBehaviour _behaviour;

    public EnemyFollowingBehavior(MovementType movement, EnemyFollowEventsModel followEventsModel)
    {
        if (movement != MovementType.Walk) throw new System.Exception("This constructor should be used only for 'MovementType' = 'Walk'");
        _movement = movement;
        EnemyFollowEventsModel = followEventsModel;
    }

    public EnemyFollowingBehavior(MovementType movement, EnemyFollowModel followingModel)
    {
        _movement = movement;
        EnemyFollowingModel = followingModel;
    }

    public void Start(Enemy enemy)
    {
        switch (_movement)
        {
            case MovementType.Walk:
                _behaviour = new Walk(this);
                break;
            case MovementType.Jump:
                _behaviour = new Jump(this);
                break;
            case MovementType.Fly:
                break;
        }
        _behaviour.Start(enemy);
    }

    public void OnEnterBehaviour() => _behaviour?.OnEnterBehaviour();
    public void OnExitBehaviour() => _behaviour?.OnExitBehaviour();
    public void Update() => _behaviour?.Update();

    public class Target
    {
        public bool NeedToJump { get; private set; }
        public bool NeedToGoDownPlatform { get; private set; }

        public Node ParentNode { get; set; }
        public Transform TargeTransform { get; set; }
        public Vector2 Position => TargeTransform.position;

        public Target JumpTarget { get; set; }

        public void ClearBooleans()
        {
            NeedToGoDownPlatform = false;
            NeedToJump = false;
        }

        public void CheckIfNeedToJump(bool isFirstIteration, Node node, float minVerticalPosition)
        {
            NeedToJump = ParentNode.neighbours.Where(e => e.node.GetInstanceID() == node.GetInstanceID() && e.needToJump).Any();
        }

        public void CheckIfNeedToGoDownPlatform(bool isFirstIteration, Node node, float minVerticalPosition, bool isOverPlatform)
        {
            NeedToGoDownPlatform =
                (node.transform.position.y < minVerticalPosition && isOverPlatform) ||
                ParentNode.neighbours.Where(e => e.node.GetInstanceID() == node.GetInstanceID() && e.needToGoDownPlatform).Any();
        }

        public void Log()
        {
            Debug.Log($"position: {Position} \t name: {TargeTransform.name}");
        }
    }

    public enum MovementType
    {
        Walk,
        Jump,
        Fly
    }
}
