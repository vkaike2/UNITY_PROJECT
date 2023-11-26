using System.Collections;
using UnityEngine;

public class FollowingFartProjectile : FartProjectile
{
    [SerializeField]
    private float _minimumDistanceToPlayer = 0.5f;
    [SerializeField]
    private float _speedMultiplier = 2f;

    private bool _startFollowingPlayer = false;

    public override void SetInitialValues(Vector2 velocity, Player player)
    {
        base.SetInitialValues(velocity, player);
        StartCoroutine(WaitToStartFollowingPLayer());
    }

    private void FixedUpdate()
    {
        if (_player == null) return;
        if (!_startFollowingPlayer) return;

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

    private IEnumerator WaitToStartFollowingPLayer()
    {
        yield return new WaitForSeconds(0.5f);
        _startFollowingPlayer = true;
    }
}
