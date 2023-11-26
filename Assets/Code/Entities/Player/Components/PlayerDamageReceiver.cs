using System;
using System.Collections;
using UnityEngine;

public class PlayerDamageReceiver : DamageReceiver
{
    private Player _player;

    protected override void AfterAwake()
    {
        _isPlayer = true;
        _player = GetComponent<Player>();
    }

    protected override void OnDie()
    {
        UIEventManager.instance.OnPlayerLifeChange.Invoke(0);
        _player.ChangeState(Player.FiniteState.Dead);
    }
}
