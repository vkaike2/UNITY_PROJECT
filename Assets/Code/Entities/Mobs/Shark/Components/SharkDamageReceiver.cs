public class SharkDamageReceiver : DamageReceiver
{
    private Shark _shark;

    protected override void AfterAwake()
    {
        _shark = GetComponent<Shark>();
    }

    protected override void OnDie(string damageSource)
    {
        _shark.ChangeBehaviour(Shark.Behaviour.Die);
    }
}