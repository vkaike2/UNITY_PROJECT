using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ToiletEnabledModel
{
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public Animator ToiletPlayerAnimator { get; private set; }

    public UnityEvent OnPlayerEnteringToiletEvent { get; private set; } = new UnityEvent();
    public UnityEvent OnToiletCompletlyClosed { get; private set; } = new UnityEvent(); 
    public OnInteractWithToiletEvent OnInteractWithToiletEvent { get; private set; } = new OnInteractWithToiletEvent();


    public void ResetEvents()
    {
        OnPlayerEnteringToiletEvent.RemoveAllListeners();
        OnToiletCompletlyClosed.RemoveAllListeners();
        OnInteractWithToiletEvent.RemoveAllListeners();
    }
}
public class OnInteractWithToiletEvent : UnityEvent<Player> { }
