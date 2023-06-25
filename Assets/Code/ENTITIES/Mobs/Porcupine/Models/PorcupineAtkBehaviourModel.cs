using Assets.Code.LOGIC;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PorcupineAtkBehaviourModel
{
    [Header("COMPONENTS")]
    [SerializeField]
    private PorcupineProjectile _projectile;
    [SerializeField]
    private Transform _projectileContainer;
    [SerializeField]
    private Transform _projectileSpawnPoint;
    [SerializeField]
    private LayerCheckCollider _groundCheckRaycast;

    [Space]
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _projectileSpeed = 5f;
    [SerializeField]
    private float _projectileDuration = 2f;
    [SerializeField]
    private float _jumpForce = 10f;
    [SerializeField]
    private float _cdwBetweenAtks = 3f;


    public PorcupineProjectile Projectile => _projectile;
    public Transform ProjectileContainer => _projectileContainer;
    public LayerCheckCollider GroundCheckRaycast => _groundCheckRaycast;

    public bool CanAtk { get; set; }
    public float ProjectileSpeed => _projectileSpeed;
    public float ProjectileDuration => _projectileDuration;
    public float JumpForce => _jumpForce;
    public float CdwBetweenAtks => _cdwBetweenAtks;
    public Transform ProjectileSpawnPoint => _projectileSpawnPoint;

    private RegisterProjectile _onRegisterProjectile = new();
    public RegisterProjectile OnRegisterProjectile => _onRegisterProjectile;

    public class RegisterProjectile : UnityEvent<PorcupineProjectile> { }
}
