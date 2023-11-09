using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class PoopExplosionVFX : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private PoopExplosionVFXParticle _particlePrefab;
    [SerializeField]
    private Transform _center;
    [field: SerializeField]
    public Hitbox Hitbox { get; private set; }
    [SerializeField]
    private CircleCollider2D _circleCollider;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _cdwToTriggerExplosion;
    [SerializeField]
    private float _explosionDuration;
    [SerializeField]
    private float _paddingRadius;

    private PlayerStatus _playerStatus;
    private PlayerDamageDealer _playerDamageDealer;

    private void Awake()
    {
    }

    public void SetInitialValues(PlayerStatus playerStatus, PlayerDamageDealer playerDamageDealer)
    {
        _playerStatus = playerStatus;
        _playerDamageDealer = playerDamageDealer;

        CreateParticles(playerStatus.Poop);

        StartCoroutine(TriggerExplosion());
        StartCoroutine(ExplosionDuration());
        Hitbox.OnHitboxTriggerEnter.AddListener(HandleExplosionColision);
    }

    public void HandleExplosionColision(Hitbox targetHitbox, Hitbox myHitbox)
    {
        _playerDamageDealer.OnApplyPoopDamage(targetHitbox, myHitbox, () => { }, true);
    }

    public void CreateParticles(PlayerStatus.PoopStatus poopStatus)
    {
        float explosionArea = poopStatus.AreaOfEffect.Get();
        SetColliderRadius(explosionArea);

        PoopExplosionVFXParticle centerParticle = Instantiate(_particlePrefab, _center.position, quaternion.identity);
        centerParticle.transform.parent = _center;
        centerParticle.SetCircleCollider(_circleCollider);

        if (explosionArea == 1 || explosionArea == 0)
        {
            return;
        }

        float radius = _circleCollider.radius;
        List<float> internalRadiuses = GetInternalRadius(radius, explosionArea);

        int particleMultiplicator = 6;
        int amoutOfParticle = particleMultiplicator;

        for (int i = 0; i < internalRadiuses.Count-1; i++)
        {
            float internalRadius = internalRadiuses[i];

            var positions = GetRandomRadiusPosition(internalRadius, amoutOfParticle);

            foreach (var position in positions)
            {
                PoopExplosionVFXParticle particle = Instantiate(_particlePrefab, position, quaternion.identity);
                particle.transform.parent = _center;
            }

            amoutOfParticle += particleMultiplicator;
        }

        return;
    }

    private void SetColliderRadius(float aditionalArea)
    {
        if (aditionalArea == 0) return;

        _circleCollider.radius *= (aditionalArea + 1);
    }

    private List<Vector2> GetRandomRadiusPosition(float internalRadius, int amount)
    {
        Vector2 center = _circleCollider.transform.position;

        List<Vector2> positions = new List<Vector2>();

        List<float> angles = new List<float>();
        float deltaAngle = (Mathf.PI * 2f) / amount;

        for (int i = 0; i < amount; i++)
        {
            angles.Add(deltaAngle * (i+1));
        }

        foreach (var angle in angles)
        {
            float xValue = center.x + internalRadius * Mathf.Cos(angle);
            float yValue = center.y + internalRadius * Mathf.Sin(angle);

            positions.Add(new Vector2(xValue, yValue));
        }

        return positions;
    }

    private List<float> GetInternalRadius(float radius, float area)
    {
        List<float> internalRadiuses = new List<float>();

        float deltaRadius = radius / area;

        float internalRadius = 0;

        int count = 1;
        while (internalRadius < radius)
        {
            internalRadius = deltaRadius * count;
            internalRadiuses.Add(deltaRadius * count);
            count++;
        }
        return internalRadiuses;
    }

    private IEnumerator TriggerExplosion()
    {
        yield return new WaitForSeconds(_cdwToTriggerExplosion);
        _circleCollider.enabled = true;
    }

    private IEnumerator ExplosionDuration()
    {
        yield return new WaitForSeconds(_explosionDuration);
        Destroy(this.gameObject);
    }
}
