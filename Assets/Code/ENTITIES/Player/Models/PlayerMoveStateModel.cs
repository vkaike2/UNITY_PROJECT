
using System;
using UnityEngine;

[Serializable]
public class PlayerMoveStateModel
{
    [SerializeField]
    private float _movementSpeed = 7f;

    public float MovementSpeed => _movementSpeed;
}
