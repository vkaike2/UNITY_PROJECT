using System.Collections;
using UnityEngine;


public class PoopProjectile : MonoBehaviour
{
    [Header("configuration")]
    [SerializeField]
    private Layer _colisionLayer;

    private Rigidbody2D _rigidBody2D;

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(Vector2 velocityDirection)
    {
        _rigidBody2D.velocity = velocityDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.layer == (int)_colisionLayer)
        {
            Destroy(this.gameObject, 0.1f);
        }
    }

    public enum Layer
    {
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Ground = 3,
        Water = 4,
        UI = 5
    }
}
