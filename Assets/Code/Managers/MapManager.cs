using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public CinemachineConfiner2D CinemachineConfiner2D { get; private set; }
    [field: SerializeField]
    public CinemachineVirtualCamera VirtualCamera { get; private set; }
    [Space]
    [SerializeField]
    private Transform _mapContainer;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private Map _test;

    public Toilet Toilet { get; set; }
    public Map CurrentMap { get; private set; } = null;
    public List<Enemy> EnemiesInsideMap { get; set; } = new List<Enemy>();


    private GameManager _gameManager;

    private void Awake()
    {
        this.transform.position = Vector3.zero;
    }

    private void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();

        _gameManager.OnPlayerDead.AddListener(OnPlayerDead);
    }

    public void SetCameraConfigurations(Collider2D mapConfiner, float cameraSize)
    {
        CinemachineConfiner2D.m_BoundingShape2D = mapConfiner;
        VirtualCamera.m_Lens.OrthographicSize = cameraSize;
    }

    public void StartNextMap(Map currentMap)
    {
        Destroy(currentMap.gameObject);
        CurrentMap = Instantiate(_test, this.transform);
        CurrentMap.transform.SetParent(_mapContainer);
    }

    private void OnPlayerDead()
    {
        foreach (var enemy in EnemiesInsideMap.Where(e => e != null))
        {
            enemy.Kill();
        }

        StartCoroutine(ChangeCameraSizeSmoothly(3, 1.5f, _gameManager.Player.transform));

        //VirtualCamera.Follow = _gameManager.Player.transform;
    }

    #region TOILET CAMERA HELPER
    public void FocusCameraOnToilet(Action callBack)
    {
        StartCoroutine(FocusOnToilet(callBack));
    }

    public void FocusCameraOnToiletInstantly()
    {
        VirtualCamera.Follow = Toilet.transform;
        VirtualCamera.m_Lens.OrthographicSize = Toilet.CameraSizeOnFocus;
    }

    private IEnumerator FocusOnToilet(Action callBack)
    {
        VirtualCamera.Follow = Toilet.transform;

        while (VirtualCamera.m_Lens.OrthographicSize >= Toilet.CameraSizeOnFocus)
        {
            VirtualCamera.m_Lens.OrthographicSize -= Time.deltaTime * 10;
            yield return new WaitForFixedUpdate();
        }

        callBack();
    }
    #endregion
    public void ReturnCameraFocusToPlayer(CameraConfiner confiner, Player player = null)
    {
        StartCoroutine(FocusCameraOnPlayer(confiner, player));
    }

    public IEnumerator ChangeCameraSizeSmoothly(float newSize, float timeMultiplier = 10, Transform focus = null)
    {
        if (newSize > VirtualCamera.m_Lens.OrthographicSize)
        {
            while (VirtualCamera.m_Lens.OrthographicSize <= newSize)
            {
                VirtualCamera.m_Lens.OrthographicSize += Time.deltaTime * timeMultiplier;
                yield return new WaitForFixedUpdate();

                if (focus != null)
                {
                    CinemachineConfiner2D.InvalidateCache();
                    VirtualCamera.Follow = focus;
                }
            }
        }
        else if (newSize < VirtualCamera.m_Lens.OrthographicSize)
        {
            while (VirtualCamera.m_Lens.OrthographicSize >= newSize)
            {
                VirtualCamera.m_Lens.OrthographicSize -= Time.deltaTime * timeMultiplier;
                yield return new WaitForFixedUpdate();
                if (focus != null)
                {
                    CinemachineConfiner2D.InvalidateCache();
                    VirtualCamera.Follow = focus;
                }
            }
        }

        VirtualCamera.m_Lens.OrthographicSize = newSize;

        CinemachineConfiner2D.InvalidateCache();
    }

    private IEnumerator FocusCameraOnPlayer(CameraConfiner confiner, Player player = null)
    {
        if (player == null)
        {
            player = _gameManager.Player;
        }

        CinemachineConfiner2D.m_BoundingShape2D = confiner.Collider;
        VirtualCamera.Follow = CurrentMap.CentralPosition;

        CinemachineConfiner2D.InvalidateCache();

        player.FreezePlayer(true);

        if (VirtualCamera.m_Lens.OrthographicSize != confiner.CameraSize)
        {
            yield return ChangeCameraSizeSmoothly(confiner.CameraSize);
        }

        VirtualCamera.Follow = player.transform;

        CinemachineConfiner2D.InvalidateCache();
        player.FreezePlayer(false);
    }
}
