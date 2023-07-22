using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class BirdAtkModel
{
    [field: SerializeField]
    public float AtkFlyingSpeed { get; private set; } = 3f;


    public Vector2 TargetPosition { get; set; }
}
