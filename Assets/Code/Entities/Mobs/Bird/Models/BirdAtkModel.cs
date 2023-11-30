using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class BirdAtkModel
{
    [field: SerializeField]
    public float AtkFlyingSpeed { get; private set; } = 3f;

    [field: SerializeField]
    public float MovementSpeedToGoUp { get; private set; } = 5f;

    public Vector2 TargetPosition { get; set; }
}
