using System;

public class ThirdMapCameraConfiner : BaseMapCameraConfiner
{
    protected override void OnChangeMap(int mapId, int changeId)
    {
        if(mapId != ConstantValues.THIRD_MAP_ID) return;

        CameraConfiner _currentConfiner;

        switch (changeId)
        {
            case ThirdMapChanges.UNLOCK_STEP_1:
                _currentConfiner = _cameraConfiners[1];
                break;
            case ThirdMapChanges.UNLOCK_STEP_2:
                _currentConfiner = _cameraConfiners[2];
                break;
            case ThirdMapChanges.UNLOCK_STEP_3:
                _currentConfiner = _cameraConfiners[3];
                break;
            case ThirdMapChanges.UNLOCK_STEP_5:
                _currentConfiner = _cameraConfiners[4];
                break;
            default:
                return;
        }

        StartCoroutine(ChangeCameraSize(_currentConfiner));
    }
}