using System;
using System.Collections.Generic;
using UnityEngine;

public class SecondMapCameraConfiner : BaseMapCameraConfiner
{
    [Header("COMPONENTS")]
    [SerializeField]
    private ScriptableMapEvents _mapEvents;
    [Space]
    [SerializeField]
    private List<CameraConfiner> _cameraConfiners;

    protected override void AfterStart()
    {
        _mapEvents.OnChangeMapEvent.AddListener(OnChangeMap);
    }

    private void OnChangeMap(int mapId, int changeId)
    {
        if (mapId != ConstantValues.SECOND_MAP_ID) return;

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP) GoToStepOne();

        if (changeId == SecondMapChanges.PLAYER_IS_MOVING_TO_SECOND_STEP) PlayerMovingToSecondStep();

        if (changeId == SecondMapChanges.UNLOCK_SECOND_STEP) UnlockSecondStep();

        if (changeId == SecondMapChanges.UNLOCK_THIRD_STEP) UnlockBossMap();

        if (changeId == SecondMapChanges.HACK_TEST_BOSS) UnlockBossMap();
    }

    private void UnlockBossMap()
    {
        CameraConfiner confiner = _cameraConfiners[4];
        _mapManager.SetCameraConfigurations(confiner.Collider, confiner.CameraSize);
    }

    private void GoToStepOne()
    {
        CameraConfiner confiner = _cameraConfiners[1];

        StartCoroutine(ChangeCameraSize(confiner));
    }

    private void PlayerMovingToSecondStep()
    {
        CameraConfiner confiner = _cameraConfiners[2];
        _mapManager.SetCameraConfigurations(confiner.Collider, confiner.CameraSize);
    }

    private void UnlockSecondStep()
    {
        CameraConfiner confiner = _cameraConfiners[3];

        StartCoroutine(ChangeCameraSize(confiner));
    }

}