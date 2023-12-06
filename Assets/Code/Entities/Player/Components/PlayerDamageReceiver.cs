using System;
using System.Collections;
using UnityEngine;

public class PlayerDamageReceiver : DamageReceiver
{
    private Player _player;


    private bool _canReceiveDamage = true;

    public void AddHealth(float amountOfHealth)
    {
        float health = _player.Status.Health.Get();

        health += amountOfHealth;
        if (health > _player.Status.MaxHealth.Get())
        {
            health = _player.Status.MaxHealth.Get();
        }

        if (_VfxParent != null)
        {
           _VfxParent.AddHealth(health);
        }

        _player.Status.Health.Set(amountOfHealth);

        this.UpdateUI();
    }

    protected override void AfterAwake()
    {
        _isPlayer = true;
        _player = GetComponent<Player>();
    }

    protected override void AfterStart()
    {
        base.AfterStart();

        this.UpdateUI();
    }

    protected override void OnReceiveDamage(float damage)
    {
        base.OnReceiveDamage(damage);

        StartCoroutine(CalculateInvincibilityCdw());
    }

    protected override bool CanReceiveDamage()
    {
        return _canReceiveDamage;
    }

    protected override void OnDie()
    {
        UIEventManager.instance.OnPlayerLifeChange.Invoke(0);
        _player.ChangeState(Player.FiniteState.Dead);
    }

    private IEnumerator CalculateInvincibilityCdw()
    {
        _canReceiveDamage = false;
        yield return new WaitForSeconds(_cdwInvincibility);
        _canReceiveDamage = true;
    }
}
