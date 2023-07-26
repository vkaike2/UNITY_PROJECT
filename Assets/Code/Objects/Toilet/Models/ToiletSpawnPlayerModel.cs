using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ToiletSpawnPlayerModel
{
    [field: Header("PREFABS")]
    [field: SerializeField]
    public Player PlayerPrefab { get; private set; }

    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public Animator ToiletPlayerAnimator { get; private set; }
    [field: SerializeField]
    public Transform PlayerSpawnPosition { get; private set; }


    public UnityEvent OnPlayerLeavingToiletEvent { get; private set; } = new UnityEvent();
    public UnityEvent OnToiletCompletlyOpen { get; private set; } = new UnityEvent();
    public UnityEvent OnToiletCompletlyClosed { get; private set; } = new UnityEvent();


    public void ResetEvents()
    {
        OnPlayerLeavingToiletEvent.RemoveAllListeners();
        OnToiletCompletlyOpen.RemoveAllListeners();
        OnToiletCompletlyClosed.RemoveAllListeners();
    }
}
