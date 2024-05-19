using System;
using UnityEngine;

[Serializable]
public class PistolCrabIdleModel
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float IdleDuration { get; set; } = 5f;
}