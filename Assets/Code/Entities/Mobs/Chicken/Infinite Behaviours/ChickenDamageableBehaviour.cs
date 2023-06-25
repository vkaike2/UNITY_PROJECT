using UnityEngine;


public class ChickenDamageableBehaviour : ChickenInfiniteBaseBehaviour
{
    private ChickenDamageableBehaviourModel _damageableModel;
    private readonly DamageService _damageService = new DamageService();

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _hitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnter);
        _damageableModel = _chicken.DamageableModel;
        _chicken.Status.InitializeHealth();

        _hitbox.OnReceivingDamage.AddListener(ReceiveDamage);

        InitializeLifeBar();
    }

    public override void Update() { }

    private void OnHitboxEnter(Hitbox targetHitbox)
    {
        if (_chicken.CurrentBehaviour == Chicken.Behaviour.Die) return;
        if (targetHitbox == null) return;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_chicken.Status.ImpactDamage.Get(), _hitbox.GetInstanceID(), _chicken.transform.position);
    }

    private void ReceiveDamage(float incomingDamage, int instance, Vector2 playerPosition)
    {
        if (_chicken.CurrentBehaviour == Chicken.Behaviour.Die) return;
        if (!_damageService.CanReceiveDamageFrom(instance)) return;

        _chicken.Status.Health.ReduceFlatValue(_damageService.CalculateDamageEntry(incomingDamage));
        if (_chicken.Status.Health.Get() <= 0)
        {
            _chicken.Status.Health.Set(0);

            _chicken.ChangeBehaviour(Chicken.Behaviour.Die);
        }

        _damageableModel.ProgresBarUI.OnSetBehaviour.Invoke(_chicken.Status.Health.Get() / _chicken.Status.MaxHealth.Get(), ProgressBarUI.Behaviour.LifeBar_Hide);

        _chicken.StartCoroutine(_damageService.ManageDamageEntry(instance));

        if (!_damageService.IsRuningAnimation)
        {
            _chicken.StartCoroutine(_damageService.TakeDamageAnimation(_chicken.SpriteRenderer));
        }
    }

    private void InitializeLifeBar() => _damageableModel.ProgresBarUI.OnSetBehaviour.Invoke(1, ProgressBarUI.Behaviour.LifeBar_Hide);
}
