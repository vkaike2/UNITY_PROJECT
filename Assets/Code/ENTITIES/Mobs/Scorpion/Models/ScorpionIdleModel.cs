using System;
using UnityEngine;

[Serializable]
public class ScorpionIdleModel
{
    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float CdwToWalk { get; private set; }
}