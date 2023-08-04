using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerPoopModel
{
    [field: Header("components")]
    [field: SerializeField]
    public PoopProjectile ProjectilePrefab { get; private set; }
    [field: SerializeField]
    public GameObject TrajectoryPrefab { get; private set; }
    [field: SerializeField]
    public Transform SpawnPoint { get; private set; }
    [field: SerializeField]
    public ProgressBarUI CdwProgressBar { get; private set; }
    
    [field: Space]
    [field: Header("projectile trajectory")]
    [field: SerializeField]
    public int NumberOfDots { get; private set; } = 50;

    [field: Space]
    [field: Header("configuration")]
    [field: SerializeField]
    public float MaximumVelocity { get; private set; } = 10;
    [field: SerializeField]
    [field: Tooltip("How many secconds it takes to go from 0 to maximum velocity")]
    public float VelocityTimer { get; private set; } = 1;
    [field: SerializeField]
    public float CdwToPoop { get; private set; } = 3f;
    [field: SerializeField]
    public float GravityWhilePooping { get; private set; } = 0.1f;

    public bool CanPoop { get; set; }

    public OnPoopSpawnedEvent OnPoopSpawned { get; set; } = new OnPoopSpawnedEvent();

    public class OnPoopSpawnedEvent : UnityEvent<PoopProjectile> { }
}
