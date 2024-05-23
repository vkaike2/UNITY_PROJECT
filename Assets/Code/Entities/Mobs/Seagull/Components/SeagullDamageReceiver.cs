public class SeagullDamageReceiver : DamageReceiver
{
    private Seagull _seagull;

    protected override void AfterAwake()
    {
        _seagull = GetComponent<Seagull>();
    }

    protected override void OnDie(string damageSource)
    {
        _seagull.ChangeBehaviour(Seagull.Behaviour.Die);
    }
}