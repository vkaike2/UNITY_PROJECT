public class StarFishDamageReceiver : DamageReceiver
{
    private StarFish _starFish;

    protected override void AfterAwake()
    {
        _starFish = GetComponent<StarFish>();
    }

    protected override void OnDie(string damageSource)
    {
        _starFish.ChangeBehaviour(StarFish.Behaviour.Die);
    }
}