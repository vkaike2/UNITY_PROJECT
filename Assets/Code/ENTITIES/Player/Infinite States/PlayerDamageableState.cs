using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDamageableState : PlayerInfiniteBaseState
{
    private PlayerDamageableStateModel _damageableModel;

    private ParticleSystem.Particle[] _fartParticles;
    private List<int> _fartParticleInstanceIds;
    private Hitbox _poopHitbox;
    private readonly DamageService _damageService = new DamageService();

    public override void Start(Player player)
    {
        base.Start(player);
        _damageableModel = player.DamageableStateModel;
        _damageableModel.CurrentHealth = _damageableModel.InitialHealth;

        player.Hitbox.OnReceivingDamage.AddListener(ReceiveDamage);
        player.OnPoopEvent = OnPlayerPoop;
        RestartLifeBar();
        InitializeParticles();
    }

    public override void Update()
    {
        if (!_damageableModel.FartParticleSystem.IsAlive()) return;

        int numParticlesAlive = _damageableModel.FartParticleSystem.GetParticles(_fartParticles);

        if (numParticlesAlive > 0)
        {
            CheckFartParticleColision();
        }
    }

    private void InitializeParticles()
    {
        int numberOfParticles = (int)_damageableModel.FartParticleSystem.emission.GetBurst(0).count.constant;
        if (_fartParticles == null || _fartParticles.Length < numberOfParticles)
        {
            _fartParticles = new ParticleSystem.Particle[numberOfParticles];
        }

        _fartParticleInstanceIds = new List<int>();

        for (int i = 0; i < numberOfParticles; i++)
        {
            _fartParticleInstanceIds.Add(Random.Range(int.MinValue, int.MaxValue));
        }
    }

    private void RestartLifeBar()
    {
        _player.UiEventManager.OnPlayerLifeChange.Invoke(1);
    }

    private void ReceiveDamage(float incomingDamage, int instance)
    {
        if (!_damageService.CanReceiveDamageFrom(instance)) return;

        _damageableModel.CurrentHealth -= incomingDamage;

        if (_damageableModel.CurrentHealth <= 0)
        {
            _damageableModel.CurrentHealth = 0;
        }

        _player.StartCoroutine(_damageService.ManageDamageEntry(instance));

        _player.UiEventManager.OnPlayerLifeChange.Invoke(_damageableModel.CurrentHealth / _damageableModel.InitialHealth);

        _player.StartCoroutine(_damageService.TakeDamageAnimation(_damageableModel.SpriteRenderer));
    }

    private void CheckFartParticleColision()
    {
        for (int i = 0; i < _fartParticles.Count(); i++)
        {
            List<RaycastHit2D> hitList = Physics2D.RaycastAll(_fartParticles[i].position, Vector2.zero).ToList();

            List<Hitbox> hitboxes = GetHitboxesOverRaycastHit(hitList);

            hitboxes = hitboxes.Where(e => e.GetInstanceID() != _player.Hitbox.GetInstanceID()).ToList();
            if (!hitboxes.Any()) continue;

            foreach (var hitbox in hitboxes)
            {
                hitbox.OnReceivingDamage.Invoke(_damageableModel.FartDamage, _fartParticleInstanceIds[i]);
            }
        }
    }

    private List<Hitbox> GetHitboxesOverRaycastHit(List<RaycastHit2D> hitsUI)
    {
        List<Hitbox> components = hitsUI.Where(e => e.collider.GetComponent<Hitbox>()).Select(e => e.collider.GetComponent<Hitbox>()).ToList();
        if (components.Any()) return components;

        components = hitsUI.Where(e => e.collider.GetComponentInChildren<Hitbox>()).Select(e => e.collider.GetComponentInChildren<Hitbox>()).ToList();
        if (components.Any()) return components;

        components = hitsUI.Where(e => e.collider.GetComponentInParent<Hitbox>()).Select(e => e.collider.GetComponentInParent<Hitbox>()).ToList();
        return components;
    }

    private void OnPlayerPoop(GameObject poopProjectile)
    {
        _poopHitbox = poopProjectile.GetComponent<Hitbox>();
        _poopHitbox.OnHitboxTriggerEnter.AddListener(OnPoopHitboxEnter);
    }

    private void OnPoopHitboxEnter(Hitbox targetHitbox)
    {
        if (targetHitbox == null) return;
        if (targetHitbox.Type == Hitbox.HitboxType.Player) return;
        if (_poopHitbox == null) return;

        targetHitbox.OnReceivingDamage.Invoke(_damageableModel.PoopDamage, _poopHitbox.GetInstanceID());

        GameObject.Destroy(_poopHitbox.gameObject);
        _poopHitbox = null;
    }

}
