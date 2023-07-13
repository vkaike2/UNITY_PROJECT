using UnityEngine.Events;

public class PorcupineDamageDealer : ImpactDamageDealer
{
    private Porcupine _porcupine;

    public RegisterProjectile OnRegisterProjectileEvent { get; private set; }

    private void Awake()
    {
        _porcupine = GetComponent<Porcupine>();
        OnRegisterProjectileEvent = new RegisterProjectile();

        OnRegisterProjectileEvent.AddListener(OnRegisterProjectile);
    }

    protected override bool ValidateHit(Hitbox targetHitbox)
    {
        if (_porcupine.CurrentBehaviour == Porcupine.Behaviour.Die) return false;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return false;

        return true;
    }

    private void OnRegisterProjectile(PorcupineProjectile projectile)
    {
        Hitbox projectileHitbox = projectile.GetComponent<Hitbox>();

        projectileHitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnterProjectile);
    }

    private void OnHitboxEnterProjectile(Hitbox targetHitbox)
    {
        if (_porcupine.CurrentBehaviour == Porcupine.Behaviour.Die) return;
        if (targetHitbox == null) return;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_porcupine.Status.ImpactDamage.Get(), ImpactHitbox.GetInstanceID(), _porcupine.transform.position);
    }

    public class RegisterProjectile : UnityEvent<PorcupineProjectile> { }
}
