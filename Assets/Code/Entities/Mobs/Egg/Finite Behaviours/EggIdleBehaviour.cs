using System.Collections;
using UnityEngine;

public class EggIdleBehaviour : EggFiniteBaseBehaviour
{
    public override Egg.Behaviour Behaviour => Egg.Behaviour.Idle;

    public override void Update()
    {
    }

    public override void OnEnterBehaviour()
    {
        _egg.StartCoroutine(WaitToSpawn());
    }

    public override void OnExitBehaviour()
    {
        _egg.StopAllCoroutines();
    }


    private IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(_egg.IdleModel.CdwToSpawn);

        _egg.ChangeBehaviour(Egg.Behaviour.Spawning);
    }
}