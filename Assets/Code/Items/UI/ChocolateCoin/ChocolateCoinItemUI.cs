using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocolateCoinItemUI : UsableItemUI
{
    public override bool CanUseItem()
    {
        return _gameManager.Player.CanEat(false);
    }

    public override void UseItem()
    {
        _itemEvents.OnUseItem.Invoke(_inventoryItemUI.ItemData.Item);
        _gameManager.Player.ChangeState(Player.FiniteState.Eating);
    }
}
