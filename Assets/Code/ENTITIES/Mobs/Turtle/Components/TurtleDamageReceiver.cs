public class TurtleDamageReceiver : DamageReceiver
{
    private Turtle _turtle;

    protected override void AfterAwake()
    {
        _turtle = GetComponent<Turtle>();
    }

    protected override void OnDie()
    {
        _turtle.ChangeBehaviour(Turtle.Behaviour.Die);
    }
}