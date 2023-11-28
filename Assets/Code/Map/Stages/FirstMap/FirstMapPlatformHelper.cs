using Calcatz.MeshPathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMapPlatformHelper : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private ScriptableMapEvents _mapEvents;
    
    [Header("SMALL STAGE")]
    [SerializeField]
    private PlatformToUnlock _firstUnlock;

    [Header("MEDIUM STAGE")]
    [SerializeField]
    private List<Node> _mediumGroundNodes;
    [SerializeField]
    private List<EnemySpawnPosition> _mediumDefaultSpawners;
    [SerializeField]
    private List<EnemySpawnPosition> _mediumAirSpawnerToDeactivate;
    [Space]
    [SerializeField]
    private PlatformToUnlock _mediumStageSmallPlatforms;
    [SerializeField]
    private PlatformToUnlock _mediumStageBigPlatform;

    [Header("LARGE STAGE")]
    [SerializeField]
    private List<Node> _largeGroundNodes;
    [SerializeField]
    private List<EnemySpawnPosition> _largeDefaultSpawners;
    [SerializeField]
    private List<EnemySpawnPosition> _largeAirSpawnerToDeactivate;
    [Space]
    [SerializeField]
    private PlatformToUnlock _largeStageSmallPlatforms;
    [SerializeField]
    private PlatformToUnlock _largeStageMiddlePlatform;
    [SerializeField]
    private PlatformToUnlock _largeStageSmallerPlatform;
    [SerializeField]
    private PlatformToUnlock _largeStageBigPlatform;

    private void Start()
    {
        _mapEvents.OnChangeMapEvent.AddListener(UnlockPlatforms);
    }

    private void UnlockPlatforms(int mapId, int changeId)
    {
        if (mapId != ConstantValues.FIRST_MAP_ID) return;

        if (changeId == FirstMapChanges.SMALL_STAGE_UNLOCK_PLATFORM) _firstUnlock.Unlock();
        if(changeId == FirstMapChanges.WALL_OF_SPIKE_READY_MEDIUM)
        {
            foreach (var node in _mediumGroundNodes)
            {
                node.traversable = true;
            }
            foreach (var spawner in _mediumDefaultSpawners)
            {
                spawner.IsAvailable = true;
            }
            foreach(var spawner in _mediumAirSpawnerToDeactivate)
            {
                spawner.IsAvailable = false;
            }
        }

        if(changeId == FirstMapChanges.MEDIUM_STAGE_UNLOCK_TWO_SMALL_PLATFORMS) _mediumStageSmallPlatforms.Unlock();
        if(changeId == FirstMapChanges.MEDIUM_STAGE_UNLOCK_BIG_PLATFORM) _mediumStageBigPlatform.Unlock();
        if (changeId == FirstMapChanges.WALL_OF_SPIKE_READY_LARGE)
        {
            foreach (var node in _largeGroundNodes)
            {
                node.traversable = true;
            }
            foreach (var spawner in _largeDefaultSpawners)
            {
                spawner.IsAvailable = true;
            }
            foreach (var spawner in _largeAirSpawnerToDeactivate)
            {
                spawner.IsAvailable = false;
            }
        }

        if (changeId == FirstMapChanges.LARGE_STAGE_UNLOCK_TWO_SMAL_PLATFORMS) _largeStageSmallPlatforms.Unlock();
        if(changeId == FirstMapChanges.LARGE_STAGE_UNLOCK_MIDDLE_PLATFORMS) _largeStageMiddlePlatform.Unlock();
        if(changeId == FirstMapChanges.LARGE_STAGE_UNLOCK_SMALLER_PLATFORMS) _largeStageSmallerPlatform.Unlock();
        if(changeId == FirstMapChanges.LARGE_STAGE_UNLOCK_BIG_PLATFORMS) _largeStageBigPlatform.Unlock();
    }

    [Serializable]
    private class PlatformToUnlock
    {
        [field: SerializeField]
        public List<OneWayPlatform> Platforms { get; private set; }
        [field: SerializeField]
        public List<Node> Nodes { get; private set; }

        [field: SerializeField]
        public List<EnemySpawnPosition> EnemySpawnPositions { get; private set; }

        public void Unlock()
        {
            foreach (OneWayPlatform platform in Platforms)
            {
                platform.gameObject.SetActive(true);
            }

            foreach (Node node in Nodes)
            {
                if(node == null) continue;
                node.traversable = true;
            }

            foreach (var spawnPosition in EnemySpawnPositions)
            {
                spawnPosition.IsAvailable = true;
            }
        }
    }
}
