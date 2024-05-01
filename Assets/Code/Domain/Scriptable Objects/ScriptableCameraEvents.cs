
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CameraEvent", menuName = "ScriptableObjects/Events/Camera")]
public class ScriptableCameraEvents : ScriptableObject
{
    public OnScreenShakeEvent OnScreenShakeEvent {get; private set;} = new OnScreenShakeEvent();
}

/// <summary>
///     GameObject - Source of ScreenShake
/// </summary>
public class OnScreenShakeEvent : UnityEvent<GameObject> {}