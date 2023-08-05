using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

public class UIEventManager : MonoBehaviour
{
    public static UIEventManager instance;

    public OnPlayerProgressBar OnPlayerLifeChange { get; set; } = new OnPlayerProgressBar();
    public OnPlayerProgressBar OnPlayerFartProgressBar { get; set; } = new OnPlayerProgressBar();
    public OnPlayerProgressBar OnPlayerPoopProgressBar { get; set; } = new OnPlayerProgressBar();
    public UnityEvent OnToggleInventoryOpen { get; set; } = new UnityEvent();
    public OnInventoryChange OnInventoryChange { get; set; } = new OnInventoryChange();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
/// <summary>
///     float -> hp percentage
/// </summary>
public class OnPlayerProgressBar : UnityEvent<float> { }

/// <summary>
///     update inventory itens 
/// </summary>
public class OnInventoryChange : UnityEvent<InventoryData, EventSentBy> { }