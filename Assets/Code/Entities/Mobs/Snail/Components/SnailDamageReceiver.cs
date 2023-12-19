public class SnailDamageReceiver : DamageReceiver
{
    private Snail _snail;

    protected override void AfterAwake()
    {
        _snail = GetComponent<Snail>();
    }

    protected override void OnDie()
    {
        _snail.ChangeBehaviour(Snail.Behaviour.Die);
    }
}
