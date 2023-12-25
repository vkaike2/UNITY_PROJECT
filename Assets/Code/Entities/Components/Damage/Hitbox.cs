using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [Header("configuration")]
    [SerializeField]
    private HitboxType _hitboxType;

    public HitboxType Type => _hitboxType;
    public OnHitboxTriggerEnter OnHitboxTriggerEnter { get; private set; } = new OnHitboxTriggerEnter();
    public OnReceivingDamage OnReceivingDamage { get; private set; } = new OnReceivingDamage();
    public Collider2D Collider => _collider;

    private bool _canCallTriggerEnter = false;
    private Coroutine _resetTriggerCoroutine;
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hitbox targetHitbox = collision.GetComponent<Hitbox>();
        if (targetHitbox == null) return;
        OnHitboxTriggerEnter.Invoke(targetHitbox, this);

        RestartTriggerCorroutine();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Hitbox targetHitbox = collision.GetComponent<Hitbox>();
        if (targetHitbox == null) return;
        if (!_canCallTriggerEnter) return;


        OnHitboxTriggerEnter.Invoke(targetHitbox, this);
        RestartTriggerCorroutine();
    }

    private void RestartTriggerCorroutine()
    {        
        if (_resetTriggerCoroutine != null)
        {
            StopCoroutine(_resetTriggerCoroutine);
        }

        if (!gameObject.activeInHierarchy) return;
        _resetTriggerCoroutine = StartCoroutine(ResetTrigger());
    }

    private IEnumerator ResetTrigger()
    {
        _canCallTriggerEnter = false;
        yield return new WaitForSeconds(1f);
        _canCallTriggerEnter = true;
    }

    public enum HitboxType
    {
        Enemy,
        Player
    }
}
/// <summary>
/// Hitbox - target hitbox
/// Hitbox - my hitbox
/// </summary>
public class OnHitboxTriggerEnter : UnityEvent<Hitbox, Hitbox> { }

/// <summary>
/// float - incoming damage
/// int - instanceiD that is dealing the damage
/// Vector2 - damage dealer position
/// String - description on who is dealing the damage
/// </summary>
public class OnReceivingDamage : UnityEvent<float, int, Vector2, string> { }