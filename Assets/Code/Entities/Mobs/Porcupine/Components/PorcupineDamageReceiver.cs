using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorcupineDamageReceiver : DamageReceiver
{
    private Porcupine _porcupine;

    protected override void AfterAwake()
    {
        _porcupine = GetComponent<Porcupine>();
    }

    protected override void OnDie()
    {
        _porcupine.ChangeBehaviour(Porcupine.Behaviour.Die);
    }
}
