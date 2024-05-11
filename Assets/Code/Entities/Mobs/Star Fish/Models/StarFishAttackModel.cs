using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class StarFishAttckModel
{
    [field: Header("POSITIONS")]
    [field: SerializeField]
    public Transform ProjectileSpawnPostion { get; set; }

    [field: Header("EVENTS")]
    [field: SerializeField]
    public UnityEvent OnSpawnProjectile {get; private set;} = new UnityEvent();

    [field: Header("PREFAB")]
    [field: SerializeField]
    public StarFishProjectile StarFishProjectilePrefab { get; set; }

    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float ProjectileSpeed { get; set; }
}