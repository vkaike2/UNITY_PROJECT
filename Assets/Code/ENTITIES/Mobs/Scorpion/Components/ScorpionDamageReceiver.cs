public class ScorpionDamageReceiver : DamageReceiver
{
    private Scorpion _scorpion;

    protected override void AfterAwake()
    {
        _scorpion = GetComponent<Scorpion>();
    }
    protected override void OnDie(string damageSource)
    {
        _scorpion.ChangeBehaviour(Scorpion.Behaviour.Die);
    }
}