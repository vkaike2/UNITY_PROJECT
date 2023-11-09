using System;
using UnityEngine;


public class PlayerDamageDealer : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private PlayerStatus _status;

    private Player _player;
    private Fart _fart;


    private void Awake()
    {
        _player = GetComponent<Player>();
        _fart = GetComponent<Fart>();
    }

    private void Start()
    {
        _fart.OnFartSpawnedEvent.AddListener(OnPlayerFart);
    }
    public bool TestIfItShouldCollide(Hitbox targetHitbox, bool friendlyFire = false)
    {
        if (targetHitbox == null) return false;
        if (!friendlyFire && targetHitbox.Type == Hitbox.HitboxType.Player) return false;
        return true;
    }

    #region POOP
    public void OnApplyPoopDamage(Hitbox targetHitbox, Hitbox myHitbox, Action callback, bool friendlyFire = false)
    {
        if (!TestIfItShouldCollide(targetHitbox, friendlyFire)) return;

        targetHitbox.OnReceivingDamage.Invoke(_status.Poop.Damage.Get(), myHitbox.GetInstanceID(), myHitbox.transform.position);

        callback();
    }
    #endregion

    #region FART
    private void OnPlayerFart(FartProjectile fart)
    {
        foreach (var particle in fart.Particles)
        {
            Hitbox hitbox = particle.GetComponent<Hitbox>();
            hitbox.OnHitboxTriggerEnter.AddListener(OnFartParticleHitboxEnter);
        }
    }
    private void OnFartParticleHitboxEnter(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (targetHitbox == null) return;
        if (targetHitbox.Type == Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_status.Fart.Damage.Get(), myHitbox.GetInstanceID(), myHitbox.transform.position);
    }
    #endregion
}
