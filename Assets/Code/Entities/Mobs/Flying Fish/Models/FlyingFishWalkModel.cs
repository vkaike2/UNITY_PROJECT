using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FlyingFishWalkModel
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float InitialVelocity { get; set; }
    [field: SerializeField]
    public float MovementDuration { get; set; }


    public UnityEvent OnStartMoving { get; private set; } = new UnityEvent();
}