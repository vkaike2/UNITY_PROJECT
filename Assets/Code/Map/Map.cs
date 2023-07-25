using Calcatz.MeshPathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Map : MonoBehaviour
{
    [field: Header("- COMPONENTS")]
    [SerializeField]
    private Collider2D _cameraConfiner;
    [field: SerializeField]
    public Animator Animator { get; private set; }

    [field: Header("parents")]
    [field: SerializeField]
    public Transform ObjectsPartent { get; private set; }
    [field: SerializeField]
    public Transform EnemiesParent { get; private set; }

    [Header("pathfinding")]
    [SerializeField]
    private Waypoints _waypoints;

    [field: Header("- CONFIGURATIONS")]
    [SerializeField]
    private float _initialCameraSize;
    [field: SerializeField]
    public ScriptableMapConfiguration MapConfiguration { get; private set; }
    [SerializeField]
    protected FiniteState _startWithState = FiniteState.Combat;

    public List<Transform> EnemySpawnPositions { get; private set; } = new List<Transform>();
    public MapManager MapManager => _mapManager;
    public GameManager GameManager => _gameManager;
    public UnityEvent OnDestroyEvent { get; private set; } = new UnityEvent();
    public UnityEvent OnMapIsReadyEvent { get; private set; } = new UnityEvent();
    public Toilet Toilet { get; private set; }

    private MapManager _mapManager;
    private GameManager _gameManager;

    private MapFiniteStateBase _currentState;
    protected List<MapFiniteStateBase> _finiteStates = new List<MapFiniteStateBase>()
    {
        new MapWaitState(),
        new MapCombatState()
    };

    protected void Awake()
    {
        Toilet = ObjectsPartent.GetComponentInChildren<Toilet>();

        InitializeEnemySpawnPosition();
    }

    protected void Start()
    {
        _mapManager = GameObject.FindObjectOfType<MapManager>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();


        if (_waypoints != null)
        {
            _gameManager.SetWaypoints(_waypoints);
        }

        _mapManager.SetInitialConfigurations(_cameraConfiner, _initialCameraSize);

        foreach (var finiteState in _finiteStates)
        {
            finiteState.Start(this);
        }

        //TODO: check if I need to remove it
        OnMapIsReadyEvent.Invoke();

        this.ChangeState(_startWithState);
    }

    private void FixedUpdate()
    {
        if (_currentState == null) return;

        _currentState.Update();
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke();
    }

    public void ChangeState(FiniteState state)
    {
        if (_currentState != null)
        {
            _currentState.OnExitState();
        }

        _currentState = _finiteStates.First(e => e.State == state);
        _currentState.EnterState();
    }

    #region ANIMATOR EVENTS
    // TODO: ?!?!?
    public void ANIMATOR_MapIsReady() => OnMapIsReadyEvent.Invoke();
    #endregion

    private void InitializeEnemySpawnPosition()
    {
        if(EnemiesParent == null) return;

        int childCount = EnemiesParent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            EnemySpawnPositions.Add(EnemiesParent.GetChild(i));
        }
    }

    public enum FiniteState
    {
        Combat,
        Wait
    }

    public enum MyAnimations
    {
        TurningOn,
        TurningOff
    }
}
