public class BirdDamageReceiver : DamageReceiver
{
    private Bird _bird;

    protected override void AfterAwake()
    {
        _bird = GetComponent<Bird>();
    }

    protected override void OnDie()
    {
        _bird.ChangeBehaviour(Bird.Behaviour.Die);
    }
}
