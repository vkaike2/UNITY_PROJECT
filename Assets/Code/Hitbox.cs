using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [Header("configuration")]
    [SerializeField]
    private HitboxType _hitboxType;

    public HitboxType Type => _hitboxType;
    public OnHitboxTriggerEnter OnHitboxTriggerEnter { get; set; }
    public OnReceivingDamage OnReceivingDamage { get; set; }
    public Collider2D Collider => _collider;

    private bool _canCallTriggerEnter = false;
    private Coroutine _resetTriggerCoroutine;
    private Collider2D _collider;

    private void Awake()
    {
        OnHitboxTriggerEnter = new OnHitboxTriggerEnter();
        OnReceivingDamage = new OnReceivingDamage();
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hitbox targetHitbox = collision.GetComponent<Hitbox>();
        if (targetHitbox == null) return;
        OnHitboxTriggerEnter.Invoke(targetHitbox);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Hitbox targetHitbox = collision.GetComponent<Hitbox>();
        if (targetHitbox == null) return;
        if (!_canCallTriggerEnter) return;


        OnHitboxTriggerEnter.Invoke(targetHitbox);

        if (_resetTriggerCoroutine != null)
        {
            StopCoroutine(_resetTriggerCoroutine);
        }
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
/// Hitbox - colliding with this
/// </summary>
public class OnHitboxTriggerEnter : UnityEvent<Hitbox> { }

/// <summary>
/// float - incoming damage
/// int - instanceiD that is dealing the damage
/// Vector2 - damage dealer position
/// </summary>
public class OnReceivingDamage : UnityEvent<float, int, Vector2> { }