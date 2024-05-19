using System;
using UnityEngine;

[Serializable]
public class PistolCrabWalkModel : EnemyWalkModel
{
    [field: Header("MY CONFIGURATIONS")]
    [field: SerializeField]
    public float CdwToGoBackToIdle { get; private set; } = 7;
}