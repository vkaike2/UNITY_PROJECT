using System;
using UnityEngine;

[Serializable]
public class ScorpionWalkModel : EnemyWalkModel
{

    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float CdwToMoveBackToIdle { get; set; }
}