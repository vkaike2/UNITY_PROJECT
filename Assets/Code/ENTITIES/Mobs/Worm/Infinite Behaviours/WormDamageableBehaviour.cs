using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WormDamageableBehaviour : WormInifiniteBaseBehaviour
{
    private WormDamageableBehavourModel _damageableModel;
    private readonly DamageService _damageService = new DamageService();

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _hitbox.OnHitboxTriggerEnter.AddListener(OnHitboxEnter);
        _damageableModel = _worm.DamageableModel;
        _damageableModel.CurrentHealth = _damageableModel.InitialHealth;

        _hitbox.OnReceivingDamage.AddListener(ReceiveDamage);

        InitializeLifeBar();
    }

    public override void Update() { }

    private void OnHitboxEnter(Hitbox targetHitbox)
    {
        if (_worm.CurrentBehaviour == Worm.Behaviour.Die) return;
        if (targetHitbox == null) return;
        if(targetHitbox.Type != Hitbox.HitboxType.Player) return;

        targetHitbox.OnReceivingDamage.Invoke(_damageableModel.InitialDamage, _hitbox.GetInstanceID(), _worm.transform.position);
    }

    private void ReceiveDamage(float incomingDamage, int instance, Vector2 playerPosition)
    {
        if (_worm.CurrentBehaviour == Worm.Behaviour.Die) return;
        if (!_damageService.CanReceiveDamageFrom(instance)) return;

        _damageableModel.CurrentHealth -= incomingDamage;

        if (_damageableModel.CurrentHealth <= 0)
        {
            _damageableModel.CurrentHealth = 0;
            _worm.ChangeBehaviour(Worm.Behaviour.Die);
        }
        _damageableModel.ProgresBarUI.OnSetBehaviour.Invoke(_damageableModel.CurrentHealth / _damageableModel.InitialHealth, ProgressBarUI.Behaviour.LifeBar_Hide);


        _worm.StartCoroutine(_damageService.ManageDamageEntry(instance));

        if (!_damageService.IsRuningAnimation)
        {
            _worm.StartCoroutine(_damageService.TakeDamageAnimation(_worm.SpriteRenderer));
        }
    }

    private void InitializeLifeBar() => _damageableModel.ProgresBarUI.OnSetBehaviour.Invoke(1, ProgressBarUI.Behaviour.LifeBar_Hide);

}
