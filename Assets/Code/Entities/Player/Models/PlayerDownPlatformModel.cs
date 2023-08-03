using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class PlayerDownPlatformModel 
{
    [field: SerializeField]
    public float CdwToDeactivateCollider { get; private set; } = 0.3f;
    [field: SerializeField]
    public BoxCollider2D PlayerCollider { get; private set; }


    public bool IsComingDown { get; set; }
}
