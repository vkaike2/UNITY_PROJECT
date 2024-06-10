using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public partial class Toilet : MonoBehaviour
{
    [field: Header("CONFIGURATIONS")]
    [field: Space]
    [field: SerializeField]
    public float CameraSizeOnFocus { get; private set; } = 3;

    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public BaseMap ParentMap { get; private set; }
    [field: Space]
    [field: SerializeField]
    public Animator Animator { get; private set; }
    [field: SerializeField]
    public GameObject PlatformCollider { get; private set; }
    [field: Space]
    [field: SerializeField]
    public PlayerInRangeCheck PlayerInRangeCheck { get; private set; }
    [field: Space]
    [field: SerializeField]
    public ToiletEnabledModel EnabledModel { get; private set; }
    [field: SerializeField]
    public ToiletSpawnPlayerModel SpawnPlayerModel { get; private set; }

    public bool CanInteractWithPlayer { get; set; } = false;
    public MapManager MapManager => _mapManager;

    private MapManager _mapManager;

    private readonly List<ToiletStateBase> _finiteStates = new List<ToiletStateBase>()
    {
        new Enabled(),
        new Disabled(),
        new SpawnPlayer()
    };

    private ToiletStateBase _currentState;

    private void Start()
    {
        _mapManager = GameObject.FindObjectOfType<MapManager>();
        _mapManager.Toilet = this;

        foreach (var state in _finiteStates)
        {
            state.Start(this);
        }
    }

    private void FixedUpdate()
    {
        if (_currentState == null) return;

        _currentState.Update();
    }

    public void SetState(FiniteState initialState)
    {
        this.ChangeState(initialState);
    }

    #region USED BY ANIMATOR
    public void ANIMATION_OnPlayerEnteringToilet()
    {
        EnabledModel.OnPlayerEnteringToiletEvent.Invoke();
    }

    public void ANIMATION_OnPlayerLeavingToilet()
    {
        SpawnPlayerModel.OnPlayerLeavingToiletEvent.Invoke();
    }

    public void ANIMATION_OnToiletCompletlyOpen()
    {
        SpawnPlayerModel.OnToiletCompletlyOpen.Invoke();
    }

    public void ANIMATION_OnToiletCompletlyClosed()
    {
        EnabledModel.OnToiletCompletlyClosed.Invoke();
        SpawnPlayerModel.OnToiletCompletlyClosed.Invoke();
    }
    #endregion

    public void ChangeState(FiniteState state)
    {
        _currentState?.OnExitState();

        _currentState = _finiteStates.First(e => e.State == state);
        _currentState.EnterState();
    }

    public void InteractiWithPlayer(Player player)
    {
        EnabledModel.OnInteractWithToiletEvent.Invoke(player);
    }

    public enum MyAnimations
    {
        Closed,
        ClosedSelected,
        Open,
        Opened,
        OpenedSelected,
        Closing,
        Disabled
    }

    public enum FiniteState
    {
        Enabled,
        Disabled,
        SpawnPlayer
    }
}
