using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PoopProjectile : MonoBehaviour
{
    public Hitbox Hitbox { get; private set; }

    protected PlayerDamageDealer _playerDamageDealer;
    protected PlayerStatus _playerStatus;
    protected Rigidbody2D _rigidBody2D;

    private readonly List<Layer> _groundCollisionLayers = new List<Layer>()
    {
        Layer.Ground, Layer.Platform
    };

    private void Awake()
    {
        BeforeAwake();
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void BeforeAwake()
    {
        Hitbox = GetComponent<Hitbox>();
    }

    public void SetInitialValues(Vector2 velocityDirection, PlayerStatus playerStatus, PlayerDamageDealer playerDamageDealer)
    {
        _rigidBody2D.velocity = velocityDirection;
        _playerStatus = playerStatus;
        _playerDamageDealer = playerDamageDealer;

        DoAfterInitialValues();
    }

    protected virtual void DoAfterInitialValues()
    {
        Hitbox.OnHitboxTriggerEnter.AddListener(OnApplyPoopDamage);
    }


    protected abstract void HandleLayerColision();


    private void OnApplyPoopDamage(Hitbox targetHitbox, Hitbox myHitbox)
    {
        _playerDamageDealer.OnApplyPoopDamage(
            targetHitbox,
            myHitbox,
            () =>
            {
                Destroy(this.gameObject);
            });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (_groundCollisionLayers.Any(e => (int)e == collision.gameObject.layer))
        {
            HandleLayerColision();
        }
    }

    public enum Layer
    {
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Ground = 3,
        Platform = 9,
        Water = 4,
        UI = 5
    }
}
