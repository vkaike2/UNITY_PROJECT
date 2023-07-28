using Cinemachine;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public CinemachineConfiner2D CinemachineConfiner2D { get; private set; }
    [field: SerializeField]
    public CinemachineVirtualCamera VirualCamera { get; private set; }
    
    [Space]
    [SerializeField]
    private Transform _mapParent;
    [Space]
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
        CinemachineConfiner2D.m_BoundingShape2D = mapConfiner;
        VirualCamera.m_Lens.OrthographicSize = cameraSize;
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
        VirualCamera.Follow = Toilet.transform;
        VirualCamera.m_Lens.OrthographicSize = Toilet.CameraSizeOnFocus;
    }

    public void ReturnFocusToPlayer()
    {
        StartCoroutine(FocusOnPlayer());
    }

    private IEnumerator FocusOnToilet(Action callBack)
    {
        VirualCamera.Follow = Toilet.transform;

        while (VirualCamera.m_Lens.OrthographicSize >= Toilet.CameraSizeOnFocus)
        {
            VirualCamera.m_Lens.OrthographicSize -= Time.deltaTime * 10;
            yield return new WaitForFixedUpdate();
        }

        callBack();
    }

    private IEnumerator FocusOnPlayer()
    {
        CinemachineConfiner2D.m_BoundingShape2D = CurrentMap.CameraConfiner.SmallCollider;
        VirualCamera.Follow = CurrentMap.CentralPosition;

        CinemachineConfiner2D.InvalidateCache();
        _gameManager.Player.FreezePlayer(true);

        while (VirualCamera.m_Lens.OrthographicSize <= CurrentMap.CameraConfiner.SmallCameraSize)
        {
            VirualCamera.m_Lens.OrthographicSize += Time.deltaTime * 10;
            yield return new WaitForFixedUpdate();
        }

        VirualCamera.m_Lens.OrthographicSize = CurrentMap.CameraConfiner.SmallCameraSize;
        VirualCamera.Follow = _gameManager.Player.transform;

        CinemachineConfiner2D.InvalidateCache();
        _gameManager.Player.FreezePlayer(false);
    }
}
