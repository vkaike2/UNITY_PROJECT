using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemySpawnPositionGroup : MonoBehaviour
{
    public void GenerateSequentialIds()
    {
        List<EnemySpawnPosition> spawnPositions = GetComponentsInChildren<EnemySpawnPosition>().ToList();

        int count = 0;

        foreach (EnemySpawnPosition pos in spawnPositions)
        {
            pos.Id = count;
            pos.RenameObject();
            count++;
        }
    }
}
