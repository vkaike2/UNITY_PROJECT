using UnityEngine;

public class GoldenJamItemUI : UsableItemUI
{
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
