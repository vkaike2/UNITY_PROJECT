using System.Collections;
using UnityEngine;


public class StickyPoopProjectile : PoopProjectile
{
    [Header("DROP")]
    [SerializeField]
    private PoopStickyDrop _drop;

    protected override void DoAfterInitialValues()
    {
        //this.Hitbox.OnHitboxTriggerEnter.AddListener(HandleCollision);
    }

    protected override void HandleLayerColision()
    {
        SpawnDrop();
    }

    private void SpawnDrop()
    {
        PoopStickyDrop drop = Instantiate(_drop, this.transform.position, Quaternion.identity);
        drop.transform.parent = null;
        drop.SetInitialValues(_playerStatus, _playerDamageDealer);

        Destroy(this.gameObject);
    }

    private void HandleCollision(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (!_playerDamageDealer.TestIfItShouldCollide(targetHitbox)) return;

        SpawnDrop();
    }
}
