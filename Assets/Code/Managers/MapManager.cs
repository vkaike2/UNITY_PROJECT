using Cinemachine;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private CinemachineConfiner2D _cinemachineConfiner2D;
    [Space]
    [SerializeField]
    private Transform _mapParent;
    [Space]
    [SerializeField]
    private CinemachineVirtualCamera _virutalCamera;
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private Map _test;

    public Toilet Toilet { get; set; }
    public Map CurrentMap { get; private set; } = null;

    private GameManager _gameManager;

    private void Awake()
    {
        this.transform.position = Vector3.zero;
    }

    private void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void SetInitialConfigurations(Collider2D mapConfiner, float cameraSize)
    {
        _cinemachineConfiner2D.m_BoundingShape2D = mapConfiner;
        _virutalCamera.m_Lens.OrthographicSize = cameraSize;
    }

    public void StartNextMap(Map currentMap)
    {
        Destroy(currentMap.gameObject);
        CurrentMap = Instantiate(_test, this.transform);
        CurrentMap.transform.SetParent(_mapParent);
    }

    public void FocusCamareOnToilet(Action callBack)
    {
        StartCoroutine(FocusOnToilet(callBack));
    }

    public void FocusCameraOnToiletInstatly()
    {
        _virutalCamera.Follow = Toilet.transform;
        _virutalCamera.m_Lens.OrthographicSize = Toilet.CameraSizeOnFocus;
    }

    public void ReturnFocusToPlayer()
    {
        StartCoroutine(FocusOnPlayer());
    }

    private IEnumerator FocusOnToilet(Action callBack)
    {
        _virutalCamera.Follow = Toilet.transform;

        while (_virutalCamera.m_Lens.OrthographicSize >= Toilet.CameraSizeOnFocus)
        {
            _virutalCamera.m_Lens.OrthographicSize -= Time.deltaTime * 10;
            yield return new WaitForFixedUpdate();
        }

        callBack();
    }

    private IEnumerator FocusOnPlayer()
    {
        _cinemachineConfiner2D.m_BoundingShape2D = CurrentMap.CameraConfiner;
        _virutalCamera.Follow = CurrentMap.CentralPosition;

        _cinemachineConfiner2D.InvalidateCache();
        _gameManager.Player.FreezePlayer(true);

        while (_virutalCamera.m_Lens.OrthographicSize <= CurrentMap.InitialCameraSize)
        {
            _virutalCamera.m_Lens.OrthographicSize += Time.deltaTime * 10;
            yield return new WaitForFixedUpdate();
        }

        _virutalCamera.m_Lens.OrthographicSize = CurrentMap.InitialCameraSize;
        _virutalCamera.Follow = _gameManager.Player.transform;

        _cinemachineConfiner2D.InvalidateCache();
        _gameManager.Player.FreezePlayer(false);
    }
}
