using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class PlayerEatModel
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float CdwToMoveOutState {  get; private set; }
}
