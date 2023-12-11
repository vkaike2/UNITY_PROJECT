using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMapCameraConfiner : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private ScriptableMapEvents _mapEvents;
    [Space]
    [SerializeField]
    private List<CameraConfiner> _cameraConfiners;

    private MapManager _mapManager;
    private GameManager _gameManager;

    private void Start()
    {
        _mapEvents.OnChangeMapEvent.AddListener(OnChangeMap);

        _mapManager = GameObject.FindObjectOfType<MapManager>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void OnChangeMap(int mapId, int changeId)
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

    private IEnumerator ChangeCameraSize(CameraConfiner confiner, Action callback = null)
    {
        //Used When it is getting bigger
        if (_mapManager.VirtualCamera.m_Lens.OrthographicSize < confiner.CameraSize)
        {
            _mapManager.CinemachineConfiner2D.m_BoundingShape2D = confiner.Collider;
        }

        if (callback == null)
        {
            callback = () =>
            {
                _mapManager.CinemachineConfiner2D.m_BoundingShape2D = confiner.Collider;
            };
        }

        yield return StartCoroutine(ChangeCameraSize(confiner.CameraSize, 3f, callback));
    }

    private IEnumerator ChangeCameraSize(float newSize, float seconds, Action callback)
    {
        float initialSize = _mapManager.VirtualCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0f;

        // Used when it is getting bigger
        while (_mapManager.VirtualCamera.m_Lens.OrthographicSize < newSize)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / seconds);
            _mapManager.VirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(initialSize, newSize, t);

            yield return new WaitForFixedUpdate();

            _mapManager.CinemachineConfiner2D.InvalidateCache();
            _mapManager.VirtualCamera.Follow = _gameManager.Player.transform;
        }

        // Used when it is getting smaller
        while (_mapManager.VirtualCamera.m_Lens.OrthographicSize > newSize)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / seconds);
            _mapManager.VirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(initialSize, newSize, t);

            yield return new WaitForFixedUpdate();

            _mapManager.VirtualCamera.Follow = _gameManager.Player.transform;
            _mapManager.CinemachineConfiner2D.InvalidateCache();
        }

        _mapManager.VirtualCamera.m_Lens.OrthographicSize = newSize;
        _mapManager.CinemachineConfiner2D.InvalidateCache();

        callback();
    }
}
