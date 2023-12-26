using System;
using UnityEngine;

public class StalactiteProjectile : MonoBehaviour
{
    [Header("CONFIGURATION")]
    [SerializeField]
    private float _gravity = 1;

    private Hitbox _hitbox;
    private Rigidbody2D _rigidBody2D;
    private LayerCheckCollider _layerCheckCollider;

    private float _damage;
    private void Awake()
    {
        _hitbox = GetComponent<Hitbox>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _layerCheckCollider = GetComponent<LayerCheckCollider>();
    }

    private void Start()
    {
        _hitbox.OnHitboxTriggerEnter.AddListener(HitboxTriggerEnter);
        _layerCheckCollider.OnLayerCheckTriggerEnter.AddListener(CollidingWithGround);
    }

    public void SetInitialValues(float damage)
    {
        _damage = damage;
        _rigidBody2D.gravityScale = _gravity;
    }

    private void CollidingWithGround(GameObject gameObject)
    {
        Destroy(this.gameObject);
    }

    private void HitboxTriggerEnter(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return;
        Debug.Log("PLAYER");

        targetHitbox.OnReceivingDamage.Invoke(_damage, myHitbox.GetInstanceID(), myHitbox.transform.position, "Stalactite Projectile");
    }

}