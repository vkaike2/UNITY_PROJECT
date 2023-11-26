using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class DamageReceiver : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    protected HealthStatus _status;
    [SerializeField]
    private Hitbox _hitbox;

    [Header("VISUAL COMPONENTS")]
    [SerializeField]
    protected ProgressBarUI _progressBarUI;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private HitNumberVFXParent _VfxParent;
    [SerializeField]
    private DpsMeterUI _dpsMeter;

    [Header("CONFIGURATION")]
    [SerializeField]
    private bool _shouldApplyKnockback = false;
    [SerializeField]
    private float _knockbackDuration = 0.3f;

    [field: Header("EVENTS")]
    [field: SerializeField]
    public OnKnockbackEvent OnKnockbackEvent { get; private set; } = new OnKnockbackEvent();

    protected bool _isPlayer = false;

    private readonly List<int> _recivingDamageFrom = new List<int>();
    private const float CDW_TO_RECEIVE_DAMAGE_FOR_EACH_INSTANCE = 0.5f;
    private bool _isReceivingDamage;
    private bool _isDead = false;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        BerofeAwake();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _status.InitializeHealth();
        AfterAwake();
    }

    private void Start()
    {
        BeforeStart();
        _hitbox.OnReceivingDamage.AddListener(ReceiveDamage);
        AfterStart();
    }

    protected abstract void OnDie();
    protected virtual void OnReceiveDamage(float damage)
    {
        if (_dpsMeter == null) return;

        _dpsMeter.AddDamage(damage);
    }

    protected virtual void BerofeAwake() { }
    protected virtual void AfterAwake() { }
    protected virtual void BeforeStart() { }
    protected virtual void AfterStart() { }
    protected virtual bool CanReceiveDamage() => true;

    private void ReceiveDamage(float incomingDamage, int instance, Vector2 entityPosition)
    {
        if (_isDead) return;
        if (!CanReceiveDamage()) return;
        if (!CanReceiveDamageFrom(instance)) return;

        if (_VfxParent != null)
        {
            _VfxParent.SpawnNumber(incomingDamage);
        }
        OnReceiveDamage(incomingDamage);
        _status.Health.Remove(CalculateDamageEntry(incomingDamage));

        if (_status.Health.Get() <= 0)
        {
            _isDead = true;
            _status.Health.Set(0);
            OnDie();
            return;
        }

        ApplyKnockBackOnDamage(entityPosition);


        if (!_isPlayer)
        {
            _progressBarUI.OnSetBehaviour.Invoke(_status.Health.Get() / _status.MaxHealth.Get(), ProgressBarUI.Behaviour.LifeBar_Hide);
        }
        else
        {
            UIEventManager.instance.OnPlayerLifeChange.Invoke(_status.Health.Get() / _status.MaxHealth.Get());
        }

        StartCoroutine(ManageDamageEntry(instance));

        if (!_isReceivingDamage)
        {
            StartCoroutine(TakeDamageAnimation());
        }
    }

    private float CalculateDamageEntry(float damage)
    {
        return damage;
    }

    private bool CanReceiveDamageFrom(int instance)
    {
        return !_recivingDamageFrom.Any(e => e == instance);
    }

    private void ApplyKnockBackOnDamage(Vector2 damagePosition)
    {
        if (!_shouldApplyKnockback) return;

        OnKnockbackEvent.Invoke(_knockbackDuration);

        Vector2 direction = (Vector2)transform.position - damagePosition;
        direction = direction.normalized;

        _rigidbody2D.velocity = _status.KnockBackForce * direction;
    }

    private IEnumerator ManageDamageEntry(int instance)
    {
        _recivingDamageFrom.Add(instance);
        yield return new WaitForSeconds(CDW_TO_RECEIVE_DAMAGE_FOR_EACH_INSTANCE);
        _recivingDamageFrom.Remove(instance);
    }

    public IEnumerator TakeDamageAnimation()
    {
        _isReceivingDamage = true;

        Color color = _spriteRenderer.color;

        int howManyTimesItWillBlink = 4;
        float blinkDuration = 0.5f;

        for (int i = 0; i < howManyTimesItWillBlink; i++)
        {
            color.a = 0;
            _spriteRenderer.color = color;
            yield return new WaitForSeconds(blinkDuration / (howManyTimesItWillBlink * 2));

            color.a = 1;
            _spriteRenderer.color = color;
            yield return new WaitForSeconds(blinkDuration / (howManyTimesItWillBlink * 2));

        }
        color.a = 1;
        _spriteRenderer.color = color;

        _isReceivingDamage = false;
    }
}

/// <summary>
///  float: time that you will be controlled by the knockback
/// </summary>
[Serializable]
public class OnKnockbackEvent : UnityEvent<float> { }
