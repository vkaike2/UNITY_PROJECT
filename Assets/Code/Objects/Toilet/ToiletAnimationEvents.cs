using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToiletAnimationEvents : MonoBehaviour
{
    [field: Header("PLAYER ANIMATOR")]
    [field: SerializeField]
    public UnityEvent OnPlayerEnteringToiletEvent { get; private set; } = new UnityEvent();
    [field: SerializeField]
    public UnityEvent OnPlayerLeavingToiletEvent { get; private set; } = new UnityEvent();
    [field: Space]
    [field: Header("TOILET ANIMATOR")]
    [field: SerializeField]
    public UnityEvent OnToiletCompletlyOpen { get; private set; } = new UnityEvent();
    [field: SerializeField]
    public UnityEvent OnToiletCompletlyClosed { get; private set; } = new UnityEvent();


    public void ANIMATION_OnPlayerEnteringToilet()
    {
        OnPlayerEnteringToiletEvent?.Invoke();
    }

    public void ANIMATION_OnPlayerLeavingToilet()
    {
        OnPlayerLeavingToiletEvent?.Invoke();
    }

    public void ANIMATION_OnToiletCompletlyOpen() => OnToiletCompletlyOpen.Invoke();
    public void ANIMATION_OnToiletCompletlyClosed() => OnToiletCompletlyClosed.Invoke();
    
}
