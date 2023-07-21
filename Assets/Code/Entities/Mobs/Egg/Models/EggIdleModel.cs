using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class EggIdleModel
{
    [field: SerializeField]
    public float CdwToSpawn { get; private set; } = 5f;
}
