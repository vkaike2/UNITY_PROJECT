using System;
using UnityEngine;

[Serializable]
public class SeagullGroundModel
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float GroundDuration { get; set; }
}