public class FlyingFishDamageReceiver : DamageReceiver
{

    private FlyingFish _flyingFish;

    protected override void AfterAwake()
    {
        _flyingFish = GetComponent<FlyingFish>();
    }

    protected override void OnDie(string damageSource)
    {
        _flyingFish.ChangeBehaviour(FlyingFish.Behaviour.Die);
    }
}