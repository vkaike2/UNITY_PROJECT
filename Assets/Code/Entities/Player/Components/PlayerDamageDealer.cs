using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerDamageDealer : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private PlayerStatus _status;
    [SerializeField]
    private ParticleSystem _fartParticleSystem;

    private Player _player;
    private Hitbox _poopHitbox;
    private ParticleSystem.Particle[] _fartParticles;
    private List<int> _fartParticleInstanceIds;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _player.OnPoopEvent = OnPlayerPoop;

        InitializeParticles();
    }

    private void FixedUpdate()
    {
        if (!_fartParticleSystem.IsAlive()) return;

        int numParticlesAlive = _fartParticleSystem.GetParticles(_fartParticles);

        if (numParticlesAlive > 0)
        {
            CheckFartParticleColision();
        }
    }

    #region POOP
    //TODO: make it receive a PoopProjectile instead of a Gamebject?
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

        targetHitbox.OnReceivingDamage.Invoke(_status.PoopDamage.Get(), _poopHitbox.GetInstanceID(), _poopHitbox.transform.position);

        //TODO: it should be destroyed here?!
        GameObject.Destroy(_poopHitbox.gameObject);
        _poopHitbox = null;
    }
    #endregion

    #region FART
    private void InitializeParticles()
    {
        int numberOfParticles = (int)_fartParticleSystem.emission.GetBurst(0).count.constant;
        if (_fartParticles == null || _fartParticles.Length < numberOfParticles)
        {
            _fartParticles = new ParticleSystem.Particle[numberOfParticles];
        }

        _fartParticleInstanceIds = new List<int>();

        for (int i = 0; i < numberOfParticles; i++)
        {
            _fartParticleInstanceIds.Add(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        }
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
                hitbox.OnReceivingDamage.Invoke(_status.FartDamage.Get(), _fartParticleInstanceIds[i], _fartParticles[i].position);
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
    #endregion
}
