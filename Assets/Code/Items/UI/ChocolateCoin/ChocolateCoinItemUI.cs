public class ChocolateCoinItemUI : UsableItemUI
{
    public override bool CanUseItem()
    {
        return _gameManager.Player.CanEat(false);
    }

    public override void UseItem()
    {
        //_itemEvents.OnUseItem.Invoke(_inventoryItemUI.ItemData.Item);

        _gameManager.Player.Eat(_inventoryItemUI.ItemData);
    }
}
