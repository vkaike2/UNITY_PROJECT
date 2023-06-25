using System;
using UnityEngine;

[Serializable]
public class WormRebornBehaviourModel
{
    [SerializeField]
    private float _reborningTime = 5f;

    public float ReborningTime => _reborningTime;
}
