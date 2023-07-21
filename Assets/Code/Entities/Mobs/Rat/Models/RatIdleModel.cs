using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class RatIdleModel
{
    [field: SerializeField]
    public float DistanceToStartFollowingPlayer { get; private set; } = 5f;

}
