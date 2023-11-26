using System;
using System.Collections;
using UnityEngine;


public class OneWayPlatform : MonoBehaviour
{
    [Header("CONFIGURATION")]
    [SerializeField]
    private Transform _upperPositionTransform;

    public float UpperPosition => _upperPositionTransform.position.y;
}
