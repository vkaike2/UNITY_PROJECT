using UnityEngine;

public class GoldenJamItemUI : UsableItemUI
{

    //public void UseItem()
    //{
    //    _itemEvents.OnUseGoldenJam.Invoke(GetInstanceID());
    //}

    //private void OnUseGoldenJam(int id)
    //{
    //    if (id != GetInstanceID()) return;

    //    _inventoryItemUI.RemoveFromInventory();
    //}

    public override bool CanUseItem()
    {
        return _inventoryItemUI.Inventory.CanUseGoldenJam();
    }

    public override void UseItem()
    {
        _inventoryItemUI.Inventory.UseGoldenJam();

        _inventoryItemUI.RemoveFromInventory();
    }
}
