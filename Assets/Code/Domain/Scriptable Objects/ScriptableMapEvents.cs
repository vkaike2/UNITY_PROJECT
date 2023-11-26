using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Events/Map")]
public class ScriptableMapEvents : ScriptableObject
{
    public OnChangeMapEvent OnChangeMapEvent { get; private set; } = new OnChangeMapEvent();
}

/// <summary>
///     int - map Id
///     int - interaction Id
/// </summary>
public class OnChangeMapEvent : UnityEvent<int, int> { }
