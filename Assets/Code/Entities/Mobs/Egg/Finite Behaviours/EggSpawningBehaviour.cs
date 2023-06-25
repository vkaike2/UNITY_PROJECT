using System.Collections;
using UnityEngine;


public class EggSpawningBehaviour : EggFiniteBaseBehaviour
{
    public override Egg.Behaviour Behaviour => Egg.Behaviour.Spawning;

    public override void OnEnterBehaviour()
    {
        Enemy enemy = GameObject.Instantiate(_egg.SpawnableModel.SpawnedEnemy,_egg.transform.position, Quaternion.Euler(0f, 0f, 0f));
        enemy.transform.parent = null;
        GameObject.Destroy(_egg.gameObject);
    }

    public override void OnExitBehaviour()
    {
    }

    public override void Update()
    {
    }
}
