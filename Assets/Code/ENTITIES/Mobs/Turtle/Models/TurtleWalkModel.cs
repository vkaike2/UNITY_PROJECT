using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TurtleWalkModel
{
    [field: Header("RAYCAST CHECKS")]
    [field: SerializeField]
    public LayerMask LayerMask { get; private set; }
    [field: Space]
    [field: SerializeField]
    public RaycastModel WillHitTheWallCheck { get; private set; }
    [field: SerializeField]
    public RaycastModel WillHitTheGround { get; private set; }

    [field: Space(2)]
    [field: SerializeField]
    public TurtleGun TurtleGun { get; private set; }


    // called when the shot animation throws a bullet
    public UnityEvent OnRestartShoot { get; private set; } = new UnityEvent();
}