using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public abstract class ImpactDamageDealer : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Hitbox _hitbox;
    [SerializeField]
    private ImpactDamageStatus _status;

    [field: Header("EVENTS")]
    [field: SerializeField]
    public OnChangeHitboxEvent OnChangeHitbox { get; set; } = new OnChangeHitboxEvent();

    protected Hitbox ImpactHitbox => _hitbox;

    private void Start()
    {
        ChangeHitbox(_hitbox);
        OnChangeHitbox.AddListener(ChangeHitbox);
        AfterStart();
    }

    protected virtual void ChangeHitbox(Hitbox hitbox)
    {
        if (hitbox == null) return;
        if (_hitbox != null)
        {
            _hitbox.OnHitboxTriggerEnter.RemoveListener(OnHitboxEnterProjectile);
        }

        _hitbox = hitbox;
        _hitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnterProjectile);
    }

    protected virtual void AfterStart() { }

    protected abstract bool ValidateHit(Hitbox targetHitbox);

    private void OnHitboxEnterProjectile(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (targetHitbox == null) return;
        if (!ValidateHit(targetHitbox)) return;

        targetHitbox.OnReceivingDamage.Invoke(_status.ImpactDamage.Get(), myHitbox.GetInstanceID(), myHitbox.transform.position, "Impact Damage");
    }

}
public class OnChangeHitboxEvent : UnityEvent<Hitbox> { }
