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
        _uiEventManager.OnPlayerLifeChange.Invoke(0);
        Debug.Log("IS DED");
    }
}
