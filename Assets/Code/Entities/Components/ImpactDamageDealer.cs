using UnityEngine;

public abstract class ImpactDamageDealer : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Hitbox _hitbox;
    [SerializeField]
    private ImpactDamageStatus _status;

    protected Hitbox ImpactHitbox => _hitbox;

    private void Start()
    {
        _hitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnterProjectile);
        AfterStart();
    }

    protected virtual void AfterStart() { }

    protected abstract bool ValidateHit(Hitbox targetHitbox);

    private void OnHitboxEnterProjectile(Hitbox targetHitbox)
    {
        if (targetHitbox == null) return;
        if(!ValidateHit(targetHitbox)) return;

        targetHitbox.OnReceivingDamage.Invoke(
            _status.ImpactDamage.Get(), 
            _hitbox.GetInstanceID(), 
            transform.position);
    }

}
