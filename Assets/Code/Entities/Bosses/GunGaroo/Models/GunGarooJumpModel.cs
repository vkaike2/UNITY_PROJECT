using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class GunGarooJumpModel
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float CdwBetweenEachJump { get; private set; }
    [field: Space]
    [field: SerializeField]
    public float SuperJumpSpeed { get; private set; }
    [field: SerializeField]
    public MinMax CdwToSuperJumpGoDown { get; private set; }
}
