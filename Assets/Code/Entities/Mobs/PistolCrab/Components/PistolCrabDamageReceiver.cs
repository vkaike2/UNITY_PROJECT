public class PistolCrabDamageReceiver : DamageReceiver
{
    private PistolCrab _pistolCrab;

    protected override void AfterAwake()
    {
        _pistolCrab = GetComponent<PistolCrab>();
    }

    protected override void OnDie(string damageSource)
    {
        _pistolCrab.ChangeBehaviour(PistolCrab.Behaviour.Die);
    }
}