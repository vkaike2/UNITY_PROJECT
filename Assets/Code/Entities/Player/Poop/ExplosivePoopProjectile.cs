using UnityEngine;


public class ExplosivePoopProjectile : PoopProjectile
{
    [Header("VFX")]
    [SerializeField]
    private PoopExplosionVFX _poopExplosionVFX;

    protected override void DoAfterInitialValues()
    {
        this.Hitbox.OnHitboxTriggerEnter.AddListener(HandleCollision);
    }

    protected override void HandleLayerColision()
    {
        SpawnExplosion();
    }

    private void SpawnExplosion()
    {
        PoopExplosionVFX vfx = Instantiate(_poopExplosionVFX, this.transform.position, Quaternion.identity);
        vfx.transform.parent = null;
        vfx.SetInitialValues(_playerStatus, _playerDamageDealer);

        Destroy(this.gameObject);
    }

    private void HandleCollision(Hitbox targetHitbox, Hitbox myHitbox)
    {
        if (!_playerDamageDealer.TestIfItShouldCollide(targetHitbox)) return;

        SpawnExplosion();
    }
}
