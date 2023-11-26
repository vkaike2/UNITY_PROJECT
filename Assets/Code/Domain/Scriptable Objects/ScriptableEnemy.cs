using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
public class ScriptableEnemy : ScriptableObject
{
    [field: Header("ENEMY TYPE")]
    [field: SerializeField]
    public EnemySpawnPosition.SpawnType SpawnType { get; private set; }

    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public EnemySpawner Spawner { get; set; }
    [field: Space]
    [field: SerializeField]
    public Enemy Enemy { get; set; }
    [field: SerializeField]
    public Sprite SpawnerSprite { get; set; }
}
