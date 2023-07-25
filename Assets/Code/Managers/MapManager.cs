using Cinemachine;
using System;
using System.Collections.Generic;
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

    public void StartNextMap(Map previousMap)
    {
        previousMap.OnDestroyEvent.AddListener(SpawnNextMap);
    }

    private void SpawnNextMap()
    {
        Map newMap = Instantiate(_test, this.transform);
        newMap.transform.SetParent(_mapParent);
    }
}
