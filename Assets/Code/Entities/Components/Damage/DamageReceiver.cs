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
    [SerializeField]
    [Tooltip("for the case when you have an atk hitbox and want to take damage too")]
    private List<Hitbox> _auxiliarHitBoxes;

    [Header("VISUAL COMPONENTS")]
    [SerializeField]
    protected ProgressBarUI _progressBarUI;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    protected PopUpVFXParent _VfxParent;
    [SerializeField]
    private DpsMeterUI _dpsMeter;

    [Header("CONFIGURATION")]
    [SerializeField]
    private bool _shouldApplyKnockback = false;
    [SerializeField]
    private float _knockbackDuration = 0.3f;
    [Header("CONFIGURATION")]
    [SerializeField]
    protected float _cdwInvincibility = 0.5f;

    [field: Header("EVENTS")]
    [field: SerializeField]
    public OnKnockbackEvent OnKnockbackEvent { get; private set; } = new OnKnockbackEvent();
    [field: SerializeField]
    public OnChangeHitboxEvent OnChangeHitbox { get; private set; } = new OnChangeHitboxEvent();

    protected bool _isPlayer = false;


    private readonly List<int> _receivingDamageFrom = new List<int>();
    private const float CDW_TO_RECEIVE_DAMAGE_FOR_EACH_INSTANCE = 0.5f;
    private bool _isReceivingDamage;
    private bool _isDead = false;
    private Rigidbody2D _rigidBody2D;

    private void Awake()
    {
        BeforeAwake();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _status.InitializeHealth();
        AfterAwake();
    }

    private void Start()
    {
        BeforeStart();
        ChangeHitbox(_hitbox);
        OnChangeHitbox.AddListener(ChangeHitbox);

        if (_auxiliarHitBoxes != null)
        {
            foreach (var hitbox in _auxiliarHitBoxes)
            {
                hitbox.OnReceivingDamage.AddListener(ReceiveDamage);
            }
        }

        AfterStart();
    }

    protected abstract void OnDie(string damageSource);
    protected virtual void OnReceiveDamage(float damage)
    {
        if (_dpsMeter == null) return;

        _dpsMeter.AddDamage(damage);
    }

    protected virtual void BeforeAwake() { }
    protected virtual void AfterAwake() { }
    protected virtual void BeforeStart() { }
    protected virtual void AfterStart() { }
    protected virtual bool CanReceiveDamage() => true;

    protected virtual void ChangeHitbox(Hitbox hitbox)
    {
        if (hitbox == null) return;
        if (_hitbox != null)
        {
            _hitbox.OnReceivingDamage.RemoveListener(ReceiveDamage);
        }

        _hitbox = hitbox;
        _hitbox.OnReceivingDamage.AddListener(ReceiveDamage);
    }

    private void ReceiveDamage(float incomingDamage, int instance, Vector2 entityPosition, string damageSource)
    {
        if (_isDead) return;
        if (!CanReceiveDamage()) return;
        if (!CanReceiveDamageFrom(instance)) return;

        if (_VfxParent != null)
        {
            _VfxParent.SetHitNumber(incomingDamage);
        }

        OnReceiveDamage(incomingDamage);
        _status.Health.Remove(CalculateDamageEntry(incomingDamage));

        if (_status.Health.Get() <= 0)
        {
            _isDead = true;
            _status.Health.Set(0);
            OnDie(damageSource);
            return;
        }

        ApplyKnockBackOnDamage(entityPosition);

        UpdateUI();

        StartCoroutine(ManageDamageEntry(instance));

        if (!_isReceivingDamage)
        {
            StartCoroutine(TakeDamageAnimation());
        }
    }

    protected void UpdateUI()
    {
        if (!_isPlayer)
        {
            _progressBarUI?.OnSetBehaviour.Invoke(_status.Health.Get() / _status.MaxHealth.Get(), ProgressBarUI.Behaviour.LifeBar_Hide);
        }
        else
        {
            UIEventManager.instance.OnPlayerLifeChange.Invoke(_status.Health.Get() / _status.MaxHealth.Get());
        }
    }

    private float CalculateDamageEntry(float damage)
    {
        return damage;
    }

    private bool CanReceiveDamageFrom(int instance)
    {
        return !_receivingDamageFrom.Any(e => e == instance);
    }

    private void ApplyKnockBackOnDamage(Vector2 damagePosition)
    {
        if (!_shouldApplyKnockback) return;

        OnKnockbackEvent.Invoke(_knockbackDuration, KnockBackSource.DamageReceiver);

        Vector2 direction = (Vector2)transform.position - damagePosition;
        direction = direction.normalized;

        _rigidBody2D.velocity = _status.KnockBackForce * direction;
    }

    private IEnumerator ManageDamageEntry(int instance)
    {
        _receivingDamageFrom.Add(instance);
        yield return new WaitForSeconds(CDW_TO_RECEIVE_DAMAGE_FOR_EACH_INSTANCE);
        _receivingDamageFrom.Remove(instance);
    }

    public IEnumerator TakeDamageAnimation()
    {
        _isReceivingDamage = true;

        Color color = _spriteRenderer.color;

        int howManyTimesItWillBlink = 4;
        float blinkDuration = 0.5f;

        int multiplier = (int)(_cdwInvincibility / 0.5f);

        howManyTimesItWillBlink *= multiplier;
        blinkDuration *= multiplier;

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
public class OnKnockbackEvent : UnityEvent<float, KnockBackSource> { }
