using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistMapBossContainer : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private ScriptableMapEvents _mapEvents;
    [Space]
    [SerializeField]
    private GunGarooContainer _gungarooContainer;

    private bool _wallOfSpikeIsOnRightPlace = false;
    private bool _cameraIsOnRightPlace = false;

    private void Start()
    {
        _mapEvents.OnChangeMapEvent.AddListener(OnChangeMap);
    }

    private void OnChangeMap(int mapId, int changeId)
    {
        if (mapId != ConstantValues.FIRST_MAP_ID) return;

        if (changeId == FirstMapChanges.PREPARE_MAP_TO_BOSS)
        {
            StartCoroutine(WaitUntilStageIsCompleted());
        }

        if (changeId == FirstMapChanges.WALL_OF_SPIKE_READY_BOSS)
        {
            _wallOfSpikeIsOnRightPlace = true;
        }

        if (changeId == FirstMapChanges.CAMERA_READY_FOR_BOSS)
        {
            _cameraIsOnRightPlace = true;
        }
    }

    private IEnumerator WaitUntilStageIsCompleted()
    {
        yield return new WaitUntil(() => _wallOfSpikeIsOnRightPlace && _cameraIsOnRightPlace);
        _gungarooContainer.SpawnBoss();
    }
}
