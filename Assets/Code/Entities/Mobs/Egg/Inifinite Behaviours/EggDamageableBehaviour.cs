using UnityEngine;

public class EggDamageableBehaviour : EggInfiniteBaseBehaviour
{
    private EggDamageableBehaviourModel _damageableModel;

    private readonly DamageService _damageService = new DamageService();

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _damageableModel = _egg.DamageableModel;
        _egg.Status.InitializeHealth();

        _hitbox.OnReceivingDamage.AddListener(ReceiveDamage);

        InitializeLifeBar();
    }

    public override void Update() { }

    private void ReceiveDamage(float incomingDamage, int instance, Vector2 playerPosition)
    {
        if (_egg.CurrentBehaviour == Egg.Behaviour.Die) return;
        if (!_damageService.CanReceiveDamageFrom(instance)) return;

        _egg.Status.Health.ReduceFlatValue(_damageService.CalculateDamageEntry(incomingDamage));
        if (_egg.Status.Health.Get() <= 0)
        {
            _egg.Status.Health.Set(0);
            _egg.ChangeBehaviour(Egg.Behaviour.Die);
        }

        _damageableModel.ProgresBarUI.OnSetBehaviour.Invoke(_egg.Status.Health.Get() / _egg.Status.MaxHealth.Get(), ProgressBarUI.Behaviour.LifeBar_Hide);


        _egg.StartCoroutine(_damageService.ManageDamageEntry(instance));

        if (!_damageService.IsRuningAnimation)
        {
            _egg.StartCoroutine(_damageService.TakeDamageAnimation(_egg.SpriteRenderer));
        }
    }

    private void InitializeLifeBar() => _damageableModel.ProgresBarUI.OnSetBehaviour.Invoke(1, ProgressBarUI.Behaviour.LifeBar_Hide);

}
