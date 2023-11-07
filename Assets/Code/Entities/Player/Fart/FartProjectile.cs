using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FartProjectile : MonoBehaviour
{
    protected Rigidbody2D _rigidbody2D;

    private FartProjectileBuilder _projectileBuilder;

    public List<FartParticle> Particles { get; private set; }

    protected Player _player;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _projectileBuilder = GetComponent<FartProjectileBuilder>();
    }

    public virtual void SetInitialValues(Vector2 impulseForce, Player player)
    {
        _rigidbody2D.AddForce(impulseForce);
        BaseBehavioyrForFart(player);
    }

    protected void BaseBehavioyrForFart(Player player)
    {
        _player = player;

        PlayerStatus.FartStatus fartStatus = player.Status.Fart;

        StartCoroutine(CalculateDuration(fartStatus.Duration.Get()));
        Particles = _projectileBuilder.CreateParticles(fartStatus);

        foreach (var particle in Particles)
        {
            particle.SetInitialValues(fartStatus.Duration.Get());
        }

        player.GetComponent<Fart>().OnFartSpawnedEvent.Invoke(this);
    }

    protected IEnumerator CalculateDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
