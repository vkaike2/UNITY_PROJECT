using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class WormRebornModel
{
    [field: SerializeField]
    public float ReborningTime { get; private set; } = 5f;
}
