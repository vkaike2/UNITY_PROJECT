using UnityEngine;


public class PorcupineDamageableBehaviour : PorcupineInfiniteBaseBehaviour
{
    private PorcupineDamageableModel _damageableModel;
    private PorcupineAtkBehaviourModel _atkModel;
    private readonly DamageService _damageService = new DamageService();

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _damageableModel = _porcupine.DamageableModel;
        _atkModel = _porcupine.AtkModel;

        _porcupine.Status.InitializeHealth();

        _atkModel.OnRegisterProjectile.AddListener(OnRegisterProjectile);
        _hitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnter);
        _hitbox.OnReceivingDamage.AddListener(ReceiveDamage);

        InitializeLifeBar();
    }

    public override void Update() { }

    private void OnRegisterProjectile(PorcupineProjectile projectile)
    {
        Hitbox projectileHitbox = projectile.GetComponent<Hitbox>();

        projectileHitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnter);
    }

    private void OnHitboxEnter(Hitbox targetHitbox)
    {
        if (_porcupine.CurrentBehaviour == Porcupine.Behaviour.Die) return;
        if (targetHitbox == null) return;
        if (targetHitbox.Type != Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_porcupine.Status.ImpactDamage.Get(), _hitbox.GetInstanceID(), _porcupine.transform.position);
    }

    private void ReceiveDamage(float incomingDamage, int instance, Vector2 playerPosition)
    {
        if (_porcupine.CurrentBehaviour == Porcupine.Behaviour.Die) return;
        if (!_damageService.CanReceiveDamageFrom(instance)) return;

        _porcupine.Status.Health.ReduceFlatValue(_damageService.CalculateDamageEntry(incomingDamage));

        if (_porcupine.Status.Health.Get() <= 0)
        {
            _porcupine.Status.Health.Set(0);
            _porcupine.ChangeBehaviour(Porcupine.Behaviour.Die);
        }
        _damageableModel.ProgresBarUI.OnSetBehaviour.Invoke(_porcupine.Status.Health.Get() / _porcupine.Status.MaxHealth.Get(), ProgressBarUI.Behaviour.LifeBar_Hide);


        _porcupine.StartCoroutine(_damageService.ManageDamageEntry(instance));

        if (!_damageService.IsRuningAnimation)
        {
            _porcupine.StartCoroutine(_damageService.TakeDamageAnimation(_porcupine.SpriteRenderer));
        }
    }

    private void InitializeLifeBar() => _damageableModel.ProgresBarUI.OnSetBehaviour.Invoke(1, ProgressBarUI.Behaviour.LifeBar_Hide);
}
