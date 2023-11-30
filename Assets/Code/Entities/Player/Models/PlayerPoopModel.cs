using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerPoopModel
{
    [field: Header("components")]
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
    public float GravityWhilePooping { get; private set; } = 0.1f;

    public bool CanPoop { get; set; }
}
