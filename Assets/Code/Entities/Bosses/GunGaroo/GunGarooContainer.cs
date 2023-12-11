using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGarooContainer : MonoBehaviour
{
    [Header("PREFAB")]
    [SerializeField]
    private GunGaroo _boss;

    [Header("POSITIONS")]
    [SerializeField]
    private Transform _spawnPosition;
    [field: Space]
    [field: SerializeField]
    public Transform StartJumpPosition { get; private set; }
    [field: SerializeField]
    public Transform EndJumpPosition { get; private set; }
    [field: Space]
    [field: SerializeField]
    public Transform MaxHeightPosition { get; private set; }

    [Header("CONTAINER")]
    [SerializeField]
    private Transform _enemyContainer;

    public void SpawnBoss()
    {
        GunGaroo boss = Instantiate(_boss, _spawnPosition.transform.position, Quaternion.identity);
        boss.transform.SetParent(_enemyContainer);
        boss.AddInitialValues(this, false);
    }
}
