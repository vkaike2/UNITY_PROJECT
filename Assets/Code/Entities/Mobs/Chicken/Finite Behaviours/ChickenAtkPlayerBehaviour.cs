
public class ChickenAtkPlayerBehaviour : ChickenFiniteBaseBehaviour
{
    public override Chicken.Behaviour Behaviour => Chicken.Behaviour.Atk_Player;

    private ChickenAtkPlayerBehaviourModel _atkPlayerModel;
    private Player _player;

    public override void OnEnterBehaviour()
    {
        _chicken.Animator.PlayAnimation(ChickenAnimatorModel.AnimationName.MeleeAtk);

        _atkPlayerModel.InteractWithPlayer = () => InteractWithPlayer();
        _atkPlayerModel.EndAtkAnimation = () => EndAtkAnimator();
    }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);

        _atkPlayerModel = _chicken.AtkPlayerModel;

        _atkPlayerModel.AtkHitbox.Collider.enabled = false;
        _player = _chicken.GameManager.Player;

        ResetInternalActions();
    }

    public override void OnExitBehaviour()
    {
        ResetInternalActions();
    }

    public override void Update()
    {
    }

    private void ResetInternalActions()
    {
        _atkPlayerModel.InteractWithPlayer = () => { };
        _atkPlayerModel.EndAtkAnimation = () => { };
    }

    private void InteractWithPlayer()
    {
        _atkPlayerModel.AtkHitbox.Collider.enabled = true;
    }

    private void EndAtkAnimator()
    {
        _atkPlayerModel.AtkHitbox.Collider.enabled = false;

        GoToPatrolBehaviour();
    }

    private void GoToPatrolBehaviour()
    {
        _chicken.ChangeBehaviour(Chicken.Behaviour.Patrol);
    }

    //private void OnHitboxEnter(Hitbox targetHitbox)
    //{
    //    if (targetHitbox == null) return;
    //    if (targetHitbox.Type != Hitbox.HitboxType.Player) return;

    //    targetHitbox.OnReceivingDamage.Invoke(_chicken.Status.AtkDamage.Get(), _atkPlayerModel.AtkHitbox.GetInstanceID(), _chicken.transform.position);
    //}

}
