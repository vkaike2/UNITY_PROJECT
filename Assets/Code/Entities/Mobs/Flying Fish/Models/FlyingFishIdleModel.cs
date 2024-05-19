using System;
using UnityEngine;

[Serializable]
public class FlyingFishIdleModel
{
    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float CdwToWalk { get; set; } = 3;
}