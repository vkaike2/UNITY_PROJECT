public class EvolvedSnailDamageReceiver : DamageReceiver
{
    private EvolvedSnail _evolvedSnail;

    protected override void AfterAwake()
    {
        _evolvedSnail = GetComponent<EvolvedSnail>();
    }

    protected override void OnDie(string damageSource)
    {
        _evolvedSnail.ChangeBehaviour(EvolvedSnail.Behaviour.Die);
    }
}