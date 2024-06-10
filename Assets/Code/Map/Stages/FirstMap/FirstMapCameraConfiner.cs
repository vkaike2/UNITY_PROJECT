using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMapCameraConfiner : BaseMapCameraConfiner
{
    protected override void OnChangeMap(int mapId, int changeId)
    {
        if (mapId != ConstantValues.FIRST_MAP_ID) return;

        if (changeId == FirstMapChanges.WALL_OF_SPIKE_MOVE_TO_MEDIUM) GoToFirstPosition();

        if (changeId == FirstMapChanges.WALL_OF_SPIKE_MOVE_TO_LARGE) GoToSecondPosition();

        if (changeId == FirstMapChanges.PREPARE_MAP_TO_BOSS) GoToBossPosition();
    }

    private void GoToFirstPosition()
    {
        CameraConfiner confiner = _cameraConfiners[1];

        StartCoroutine(ChangeCameraSize(confiner));
    }

    private void GoToSecondPosition()
    {
        CameraConfiner confiner = _cameraConfiners[2];

        StartCoroutine(ChangeCameraSize(confiner));
    }

    private void GoToBossPosition()
    {
        CameraConfiner confiner = _cameraConfiners[0];

        StartCoroutine(ChangeCameraSize(confiner, () =>
        {
            _mapManager.CinemachineConfiner2D.m_BoundingShape2D = confiner.Collider;

            _mapEvents.OnChangeMapEvent.Invoke(ConstantValues.FIRST_MAP_ID, FirstMapChanges.CAMERA_READY_FOR_BOSS);
        }));
    }
}
