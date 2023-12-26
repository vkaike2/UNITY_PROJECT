public class StoneBallDamageReceiver : DamageReceiver
{
    protected override void OnDie(string damageSource)
    {
        Destroy(this.gameObject);
    }
}