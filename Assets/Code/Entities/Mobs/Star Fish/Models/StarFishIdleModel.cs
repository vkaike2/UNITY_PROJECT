using System;
using UnityEngine;

[Serializable]
public class StarFishIdleModel
{
    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float Duration {get; set;} = 5f;
}