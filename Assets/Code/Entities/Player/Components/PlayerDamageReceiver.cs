using System;
using System.Collections;
using UnityEngine;

public class PlayerDamageReceiver : DamageReceiver
{
    private Player _player;


    private bool _canReceiveDamage = true;

    protected override void AfterAwake()
    {
        _isPlayer = true;
        _player = GetComponent<Player>();
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
