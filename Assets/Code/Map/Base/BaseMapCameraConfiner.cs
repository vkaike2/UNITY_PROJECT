using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMapCameraConfiner : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    protected ScriptableMapEvents _mapEvents;
    [Space]
    [SerializeField]
    protected List<CameraConfiner> _cameraConfiners;

    protected MapManager _mapManager;
    protected GameManager _gameManager;

    private void Awake()
    {
        _mapEvents.OnChangeMapEvent.AddListener(OnChangeMap);
    }

    private void Start()
    {
        _mapManager = GameObject.FindObjectOfType<MapManager>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        AfterStart();
    }

    protected virtual void AfterStart() { }

    protected abstract void OnChangeMap(int mapId, int changeId);

    protected IEnumerator ChangeCameraSize(CameraConfiner confiner, Action callback = null)
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