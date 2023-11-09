using Calcatz.MeshPathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Map : MonoBehaviour
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public ScriptableMapConfiguration MapConfiguration { get; private set; }
    [Space]
    [SerializeField]
    private FiniteState _startWithState = FiniteState.Idle;
    
    [field: Space]
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public Transform CentralPosition { get; private set; }
    [field: Space]
    [field: SerializeField]
    public CameraConfinerInformation CameraConfiner { get; private set; }
    
    [field: Space]
    [field: SerializeField]
    public MapContainersInformation Conatiners { get; private set; }

    [field: Space]
    [Header("PATHFINDING")]
    [SerializeField]
    private Waypoints _waypoints;

    [field: Space]
    [field: Header("WALLS")]
    [field: SerializeField]
    public List<WallOfSpike> WallsOfSpike { get; private set; }

    public List<Node> Nodes { get; set; }
    public GameManager GameManager { get; private set; }
    public MapManager MapManager { get; private set; }

    public Toilet Toilet { get; private set; }

    private MapFiniteStateBase _currentState;

    protected List<MapFiniteStateBase> _finiteStates = new List<MapFiniteStateBase>()
    {
        new Combat(), // handle monster pawning
        new Wait(),   // handle what happens after combat
        new Idle()    // spawn player and move to combat
    };

    protected void Awake()
    {
        Toilet = Conatiners.GetToilet();
        Conatiners.InitializeEnemySpawnPosition();
        InitializeNodes();
    }

    protected void Start()
    {
        MapManager = GameObject.FindObjectOfType<MapManager>();
        GameManager = GameObject.FindObjectOfType<GameManager>();

        InitializeMap();

        SetWayPointToGameManager();

        foreach (var finiteState in _finiteStates)
        {
            finiteState.Start(this);
        }

        this.ChangeState(_startWithState);
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
        MapManager.SetInitialConfigurations(CameraConfiner.SmallCollider, CameraConfiner.SmallCameraSize);
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

        public List<EnemySpawnPosition> EnemySpawnPositions { get; private set; } = new List<EnemySpawnPosition>();

        public void InitializeEnemySpawnPosition()
        {
            EnemySpawnPositions = EnemiesContainer.GetComponentsInChildren<EnemySpawnPosition>().ToList();
        }

        public Toilet GetToilet()
        {
            return ObjectsContainer.GetComponentInChildren<Toilet>();
        }
    }

    [Serializable]
    public class CameraConfinerInformation
    {
        [field: SerializeField]
        public Collider2D SmallCollider { get; private set; }
        [field: SerializeField]
        public float SmallCameraSize { get; private set; }
        
        [field: Space]
        [field: SerializeField]
        public Collider2D MediumCollider { get; private set; }
        [field: SerializeField]
        public float MediumCameraSize { get; private set; }

        [field: Space]
        [field: SerializeField]
        public Collider2D LargeCollider { get; private set; }
        [field: SerializeField]
        public float LargeCameraSize { get; private set; }
    }
}
