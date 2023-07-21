using System;
using UnityEngine;


[Serializable]
public class EggSpawningModel
{
    [field: SerializeField]
    public Enemy SpawnedEnemy { get; private set; }
}
