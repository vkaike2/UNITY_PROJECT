using System;
using UnityEngine;

[Serializable]
public class TurtleAttackModel
{
    [field: Header("GUN")]
    [field: SerializeField]
    public TurtleGun TurtleGun { get; private set; }

    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float CdwBetweenShoots { get; set; } = 3f;
    [field: SerializeField]
    public float ProjectileSpeed { get; set; } = 8f;
    [field: SerializeField]
    public float ProjectileDuration { get; set; } = 7f;
}