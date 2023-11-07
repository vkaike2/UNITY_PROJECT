using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenJamItemUI : MonoBehaviour
{
    [SerializeField]
    private ScriptableItemEvents _itemEvents;

    private InventoryItemUI _iventoryItemUI;

    private void Awake()
    {
        _iventoryItemUI = GetComponent<InventoryItemUI>();

        _itemEvents.OnUseGoldenJam.AddListener(OnUseGoldenJam);
    }

    public void UseItem()
    {
        _itemEvents.OnUseGoldenJam.Invoke(GetInstanceID());
    }

    private void OnUseGoldenJam(int id)
    {
        if (id != GetInstanceID()) return;

        _iventoryItemUI.RemoveFromInventory();
    }
}
