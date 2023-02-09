using UnityEngine;
using UnityEngine.Events;

public class UIEventManager : MonoBehaviour
{
    public OnPlayerLifeChange OnPlayerLifeChange { get; set; }

    private void Awake()
    {
        OnPlayerLifeChange = new OnPlayerLifeChange();
    }
}
/// <summary>
///     float -> hp percentage
/// </summary>
public class OnPlayerLifeChange : UnityEvent<float> { }