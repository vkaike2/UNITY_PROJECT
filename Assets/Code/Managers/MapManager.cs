using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Toilet _toilet;
    [SerializeField]
    private CinemachineConfiner2D _cinemachineConfiner2D;
    [Space]
    [SerializeField]
    private Transform _mapParent;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private Map _initialMap;
    [SerializeField]
    private Map _test;

    public Toilet Toilet => _toilet;

    private GameManager _gameManager;

    private void Awake()
    {
        this.transform.position = Vector3.zero;
    }

    private void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();

        Map initialMap = Instantiate(_initialMap);
        initialMap.transform.SetParent(_mapParent);
    }

    public void SetMapConfiner(Collider2D confiner)
    {
        _cinemachineConfiner2D.m_BoundingShape2D = confiner;
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
