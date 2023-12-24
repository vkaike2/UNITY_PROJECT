using UnityEngine;
using UnityEngine.Events;

public class EnemyWalkModel
{
    [field: Header("RAYCAST CHECKS")]
    [field: SerializeField]
    public LayerMask LayerMask { get; private set; }
    [field: Space]
    [field: SerializeField]
    public RaycastModel WillHitTheWallCheck { get; private set; }
    [field: SerializeField]
    public RaycastModel WillHitTheGround { get; private set; }


    public UnityEvent OnChangeAnimationToWalk { get; private set; } = new UnityEvent();
}