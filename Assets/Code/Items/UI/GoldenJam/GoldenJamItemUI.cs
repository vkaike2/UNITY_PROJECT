using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenJamItemUI : MonoBehaviour
{
    [SerializeField]
    private ScriptableItemEvents _itemEvents;

    private InventoryItemUI _inventoryItemUI;

    private void Awake()
    {
        _inventoryItemUI = GetComponent<InventoryItemUI>();

        _itemEvents.OnUseGoldenJam.AddListener(OnUseGoldenJam);
    }

    public void UseItem()
    {
        _itemEvents.OnUseGoldenJam.Invoke(GetInstanceID());
    }

    private void OnUseGoldenJam(int id)
    {
        if (id != GetInstanceID()) return;

        _inventoryItemUI.RemoveFromInventory();
    }
}
