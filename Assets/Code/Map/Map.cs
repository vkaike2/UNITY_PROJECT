using Calcatz.MeshPathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Map : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private ScriptableMapConfiguration _mapConfiguration;
    [Space]
    [SerializeField]
    private Collider2D _cameraConfiner;
    [Space]
    [SerializeField]
    private Transform _objectsParent;
    [SerializeField]
    private Transform _enemiesParent;
    [Space]
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Waypoints _waypoints;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    protected FiniteState _startWithState = FiniteState.Combat;

    public ScriptableMapConfiguration MapConfiguration => _mapConfiguration;
    public List<Transform> EnemySpawnPositions { get; private set; } = new List<Transform>();
    public Transform EnemiesParent => _enemiesParent;
    public Transform ObjectsPartent => _objectsParent;
    public Animator Animator => _animator;
    public MapManager MapManager => _mapManager;
    public GameManager GameManager => _gameManager;

    public UnityEvent OnDestroyEvent { get; private set; } = new UnityEvent();
    public UnityEvent OnMapIsReadyEvent { get; private set; } = new UnityEvent();

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

        _mapManager.SetMapConfiner(_cameraConfiner);

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
    public void ANIMATOR_MapIsReady() => OnMapIsReadyEvent.Invoke();
    #endregion

    private void InitializeEnemySpawnPosition()
    {
        if(_enemiesParent == null) return;

        int childCount = _enemiesParent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            EnemySpawnPositions.Add(_enemiesParent.GetChild(i));
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
