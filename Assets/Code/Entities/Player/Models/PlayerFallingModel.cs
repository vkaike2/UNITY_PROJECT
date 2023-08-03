using System;
using UnityEngine;

[Serializable]
public class PlayerFallingModel 
{
    [field: SerializeField]
    public float GravityFalling { get; private set; } = 4;
}
