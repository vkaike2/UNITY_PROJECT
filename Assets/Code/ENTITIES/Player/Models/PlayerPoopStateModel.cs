using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class PlayerPoopStateModel
{
    [Header("components")]
    [SerializeField]
    private GameObject _poopProjectilePrefab;
    [SerializeField]
    private GameObject _projectileTrajectoryPrefab;
    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    private Transform _trajectoryParent;
    [SerializeField]
    private ProgressBarUI _cdwProgressBar;
    
    [Space]
    [Header("projectile trajectory")]
    [SerializeField]
    private int _numberOfDots = 50;

    [Space]
    [Header("configuration")]
    [SerializeField]
    private float _maximumVelocity = 10;
    [SerializeField]
    [Tooltip("How many secconds it takes to go from 0 to maximum velocity")]
    private float _velocityTimer = 1;
    [SerializeField]
    private float _cdwToPoop = 3f;
    [SerializeField]
    private float _gravityWhilePooping = 0.1f;

    public GameObject Prefab => _poopProjectilePrefab;
    public GameObject TrajectoryPrefab => _projectileTrajectoryPrefab;
    public Transform SpawnPoint => _spawnPoint;
    public Transform TrajectoryParent => _trajectoryParent;
    public ProgressBarUI CdwProgressBar => _cdwProgressBar;

    public int NumberOfDots => _numberOfDots;

    public float MaximumVelocity => _maximumVelocity;
    public float VelocityTimer => _velocityTimer;
    public float CdwToPoop => _cdwToPoop;
    public float GravityWhilePooping => _gravityWhilePooping;

    public bool CanPoop { get; set; }
}
