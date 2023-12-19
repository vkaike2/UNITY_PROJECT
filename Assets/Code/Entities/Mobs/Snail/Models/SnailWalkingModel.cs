using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SnailWalkingModel 
{
    [field: Header("CHECKS")]
    [field: SerializeField]
    public LayerMask LayerToCheck { get; private set; }
    [field: SerializeField]
    public Transform UpCheck { get; private set; }
    [field: SerializeField]
    public Transform DownCheck { get; private set; }
    [field: SerializeField]
    public Transform LeftCheck { get; private set; }
    [field: SerializeField]
    public Transform RightCheck { get; private set; }
    [field: Space]
    
    [field: SerializeField] 
    public Transform UpRightCheck { get; private set; }
    [field: SerializeField] 
    public Transform UpLeftCheck { get; private set; }
    [field: SerializeField] 
    public Transform DownRightCheck { get; private set; }
    [field: SerializeField] 
    public Transform DownLeftCheck { get; private set; }


    [field: Space]
    [field: SerializeField]
    public Transform IsNotTouchingGroundCheck { get; private set; }
}
