using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TurtleWalkModel : EnemyWalkModel
{
    // called when the shot animation throws a bullet
    public UnityEvent OnRestartShoot { get; private set; } = new UnityEvent();
}