using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FartProjectileBuilder : MonoBehaviour
{
    [SerializeField]
    private FartParticle _particlePrefab;
    [SerializeField]
    private Transform _center;
    [SerializeField]
    private CircleCollider2D _circleCollider;

    private readonly List<FartParticle> _particles = new List<FartParticle>();

    public List<FartParticle> CreateParticles(PlayerStatus.FartStatus fartStatus)
    {
        SetColliderRadius(fartStatus.AreaOfEffect.Get());
        for (int i = 0; i < fartStatus.AmountOfParticle.Get(); i++)
        {
            Vector2 position = GetRandomPosition();
            FartParticle particle = Instantiate(_particlePrefab, position, quaternion.identity);
            particle.transform.parent = _center;

            _particles.Add(particle);
        }

        return _particles;
    }

    private void SetColliderRadius(float aditionalArea)
    {
        if (aditionalArea == 0 || aditionalArea == 1) return;

        _circleCollider.radius *= (aditionalArea);
    }

    private Vector2 GetRandomPosition()
    {
        // Get the center and radius of the circle collider
        Vector2 center = _circleCollider.transform.position;
        float radius = _circleCollider.radius;

        // Generate random polar coordinates within the circle's radius
        float randomRadius = UnityEngine.Random.Range(0f, radius);
        float randomAngle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

        // Calculate random position using polar coordinates
        float randomX = center.x + randomRadius * Mathf.Cos(randomAngle);
        float randomY = center.y + randomRadius * Mathf.Sin(randomAngle);

        // Create the random Vector2 position
        return new Vector2(randomX, randomY);
    }
}
