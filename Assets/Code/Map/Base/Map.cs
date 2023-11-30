using Calcatz.MeshPathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public partial class Map : MonoBehaviour
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public ScriptableMapConfiguration MapConfiguration { get; private set; }
    [field: SerializeField]
    public ScriptableMapEvents MapEvents { get; private set; }
    [Space]
    [SerializeField]
    [Tooltip("Idle: spawn player and move to combat\nCombat: spawn monsters\nWait: wait player go to next stage")]
    private FiniteState _startWithState = FiniteState.Idle;

    [field: Space]
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public Transform CentralPosition { get; private set; }

    [field: Space]
    [field: SerializeField]
    public CameraConfiner CameraConfiner { get; private set; }

    [field: Space]
    [field: SerializeField]
    public MapContainersInformation Containers { get; private set; }

    [field: Space]
    [Header("PATHFINDING")]
    [SerializeField]
    private Waypoints _waypoints;

    [Header("DEBUGGING")]
    [SerializeField]
    private TMP_Text _feedBackText;

    public List<Node> Nodes { get; set; }
    public GameManager GameManager { get; private set; }
    public MapManager MapManager { get; private set; }
    public UIEventManager UIEventManager { get; private set; }

    public Toilet Toilet { get; private set; }
    public Chest Chest { get; private set; }

    private MapFiniteStateBase _currentState;

    protected List<MapFiniteStateBase> _finiteStates = new List<MapFiniteStateBase>()
    {
        new Combat(), // handle monster pawning
        new Wait(),   // handle what happens after combat
        new Idle()    // spawn player and move to combat
    };

    private void Awake()
    {
        Toilet = Containers.GetToilet();
        Chest = Containers.GetChest();

        Containers.InitializeEnemySpawnPosition();
        InitializeNodes();
    }

    private void Start()
    {
        BeforeStart();

        MapManager = GameObject.FindObjectOfType<MapManager>();
        GameManager = GameObject.FindObjectOfType<GameManager>();
        UIEventManager = GameObject.FindObjectOfType<UIEventManager>();

        InitializeMap();

        SetWayPointToGameManager();

        StartCoroutine(WaitForMapComponentsToStart());
    }

    protected virtual void BeforeStart()
    {

    }

    public void DisplayFeedBack(string message)
    {
        if (_feedBackText == null) return;
        _feedBackText.text = message;
    }

    private void FixedUpdate()
    {
        if (_currentState == null) return;

        _currentState.Update();
    }

    private void InitializeMap()
    {
        if (_startWithState != FiniteState.Wait) return;
        // add this to map manager if is the initial map
        MapManager.SetCameraConfigurations(CameraConfiner.Collider, CameraConfiner.CameraSize);
    }

    private void SetWayPointToGameManager()
    {
        if (_waypoints == null) return;
        GameManager.SetWaypoints(_waypoints);
    }

    public void ChangeState(FiniteState state)
    {
        _currentState?.OnExitState();

        _currentState = _finiteStates.First(e => e.State == state);
        _currentState.EnterState();
    }

    private void InitializeNodes()
    {
        Nodes = _waypoints.GetComponentsInChildren<Node>().ToList();
    }

    private IEnumerator WaitForMapComponentsToStart()
    {
        yield return null; // wait one frame

        foreach (var finiteState in _finiteStates)
        {
            finiteState.Start(this);
        }

        this.ChangeState(_startWithState);
    }

    public enum FiniteState
    {
        Combat,
        Wait,
        Idle
    }

    [Serializable]
    public class MapContainersInformation
    {
        [field: SerializeField]
        public Transform ObjectsContainer { get; private set; }
        [field: SerializeField]
        public Transform EnemiesContainer { get; private set; }
        [field: SerializeField]
        public Transform ItemContainer { get; private set; }

        public List<EnemySpawnPosition> EnemySpawnPositions { get; private set; } = new List<EnemySpawnPosition>();

        public void InitializeEnemySpawnPosition()
        {
            EnemySpawnPositions = EnemiesContainer.GetComponentsInChildren<EnemySpawnPosition>().ToList();
        }

        public Toilet GetToilet()
        {
            return ObjectsContainer.GetComponentInChildren<Toilet>();
        }

        public Chest GetChest()
        {
            return ObjectsContainer.GetComponentInChildren<Chest>();
        }
    }
}
