
using System;
using UnityEngine;

[Serializable]
public class ArmadilloShockwaveModel
{
    [field: Header("POSITIONS")]
    [field: SerializeField]
    public Transform PositionToStartShockwave { get; set; }

    [field: Header("PREFAB")]
    [field: SerializeField]
    public ShockwaveParent ShockwaveParentPrefab { get; set; }

    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float StateDuration { get; set; } = 3f;
}