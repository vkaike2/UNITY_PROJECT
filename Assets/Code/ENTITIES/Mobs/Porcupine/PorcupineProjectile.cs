using System;
using System.Collections;
using UnityEngine;


public class PorcupineProjectile : MonoBehaviour
{
    private Rigidbody2D _rigidBody2D;
    private LayerCheckCollider _layerCheckCollider;

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _layerCheckCollider = GetComponent<LayerCheckCollider>();
    }

    private void Start()
    {
        _layerCheckCollider.OnLayerCheckTriggerEnter.AddListener(DestroyProjectileOnCollision);
    }

    private void DestroyProjectileOnCollision(GameObject arg0)
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        float angle = Mathf.Atan2(_rigidBody2D.velocity.y, _rigidBody2D.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
    }

    public void SetInitialInitialValues(Vector2 velocity, float duration)
    {
        _rigidBody2D.velocity = velocity;
    }
}
