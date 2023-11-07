using System.Collections;
using UnityEngine;

public class FollowingFartProjectile : FartProjectile
{
    [SerializeField]
    private float _minimumDistanceToPlayer = 0.5f;
    [SerializeField]
    private float _speedMultiplier = 2f;

    public override void SetInitialValues(Vector2 velocity, Player player)
    {
        BaseBehavioyrForFart(player);
    }

    private void FixedUpdate()
    {
        if (_player == null) return;

        Vector2 direction = (_player.transform.position - transform.position).normalized;

        if (Vector2.Distance(transform.position, _player.transform.position) <= _minimumDistanceToPlayer)
        {
            _rigidbody2D.velocity = Vector2.zero;
        }
        else
        {
            _rigidbody2D.velocity = direction * _speedMultiplier;
        }
    }
}
