
using System;
using UnityEngine;

[Serializable]
public class MoveStateModel
{
    [SerializeField]
    private float _movementSpeed = 7f;

    public float MovementSpeed => _movementSpeed;
}
