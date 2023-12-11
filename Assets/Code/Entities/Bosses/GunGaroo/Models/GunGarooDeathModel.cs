using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class GunGarooDeathModel 
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public Hitbox Hitbox { get; private set; }
}
