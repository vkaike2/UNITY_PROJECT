public class RatDamageReceiver : DamageReceiver
{
    private Rat _rat;

    protected override void AfterAwake()
    {
        _rat = GetComponent<Rat>();
    }

    protected override void OnDie(string damageSource)
    {
        _rat.ChangeBehaviour(Rat.Behaviour.Die);
    }

    protected override void OnReceiveDamage(float damage)
    {
        if (_rat.CurrentBehaviour == Rat.Behaviour.Idle)
        {
            _rat.ChangeBehaviour(Rat.Behaviour.FollowingPlayer);
        }
    }
}
