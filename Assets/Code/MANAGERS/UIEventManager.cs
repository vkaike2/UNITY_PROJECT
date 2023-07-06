using UnityEngine;
using UnityEngine.Events;

public class UIEventManager : MonoBehaviour
{
    public OnPlayerLifeChange OnPlayerLifeChange { get; set; }
    public UnityEvent OnToggleInventoryOpen { get; set; }
    public OnInventoryChange OnInventoryChange { get; set; }

    private void Awake()
    {
        OnPlayerLifeChange = new OnPlayerLifeChange();
        OnToggleInventoryOpen = new UnityEvent();
        OnInventoryChange = new OnInventoryChange();
    }
}
/// <summary>
///     float -> hp percentage
/// </summary>
public class OnPlayerLifeChange : UnityEvent<float> { }

/// <summary>
///     update inventory itens 
/// </summary>
public class OnInventoryChange : UnityEvent<InventoryData, EventSentBy> { }