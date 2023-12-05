using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class RatIdleModel
{
    [field: Header("UI COMPONENTS")]
    [field: SerializeField]
    public CdwIndicationUI CdwIndicationUI { get; private set; }

    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float DistanceToStartFollowingPlayer { get; private set; } = 5f;

    [field: SerializeField]
    public float CdwToStartFollowingPlayer { get; private set; } = 10f;

}
