using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
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
    }

    private void GoToFirstPosition()
    {
        CameraConfiner confiner = _cameraConfiners[0];

        StartCoroutine(ChangeCameraSize(confiner));
    }

    private void GoToSecondPosition()
    {
        CameraConfiner confiner = _cameraConfiners[1];

        StartCoroutine(ChangeCameraSize(confiner));
    }

    private IEnumerator ChangeCameraSize(CameraConfiner confiner)
    {
        _mapManager.CinemachineConfiner2D.m_BoundingShape2D = confiner.Collider;

        yield return StartCoroutine(ChangeCameraSize(confiner.CameraSize, 1f, () =>
        {
            _mapManager.CinemachineConfiner2D.m_BoundingShape2D = confiner.Collider;
        }));

    }

    private IEnumerator ChangeCameraSize(float newSize, float timeMultiplier, Action callback)
    {
        while (_mapManager.VirtualCamera.m_Lens.OrthographicSize <= newSize)
        {
            _mapManager.VirtualCamera.m_Lens.OrthographicSize += Time.deltaTime * timeMultiplier;
            yield return new WaitForFixedUpdate();

            _mapManager.CinemachineConfiner2D.InvalidateCache();
            _mapManager.VirtualCamera.Follow = _gameManager.Player.transform;
        }

        _mapManager.VirtualCamera.m_Lens.OrthographicSize = newSize;

        _mapManager.CinemachineConfiner2D.InvalidateCache();

        callback();
    }
}
