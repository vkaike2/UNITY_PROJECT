using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BirdIdleModel 
{
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public List<BoxCollider2D> MainColliders { get; private set; }
    [field: SerializeField]
    public LayerCheckCollider GroundCheck { get; private set; }

    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float CdwToFlyBack { get; private set; }
}
