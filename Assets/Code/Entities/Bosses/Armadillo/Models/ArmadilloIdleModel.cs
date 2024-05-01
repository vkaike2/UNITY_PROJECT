
using System;
using UnityEngine;

[Serializable]
public class ArmadilloIdleModel
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float CdwToChangeState { get; set; } = 2;
}