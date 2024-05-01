public class ArmadilloDamageReceiver : DamageReceiver
{
    private Armadillo _armadillo;

    protected override void AfterAwake()
    {
        base.AfterAwake();
        _armadillo = GetComponent<Armadillo>();
    }

    protected override void OnDie(string damageSource)
    {
        _armadillo.ChangeBehaviour(Armadillo.Behaviour.Death);
    }
}