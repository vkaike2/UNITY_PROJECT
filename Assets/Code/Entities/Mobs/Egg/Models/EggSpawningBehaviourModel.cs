using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class EggSpawningBehaviourModel
{
    [SerializeField]
    private Enemy _spawnedEnemy;

    public Enemy SpawnedEnemy => _spawnedEnemy;
}
