using UnityEngine;
using UnityEngine.UI;

public class RotationalJamItemUI : TwoStepsUsableItemUI
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Image _itemImage;

    private ItemState _currentState = ItemState.Idle;
    private InventoryItemUI _iventoryItemUI;

    public override Sprite DraggableSprite => _iventoryItemUI.ItemData.Item.SpriteWhileUsing;

    private void Awake()
    {
        _iventoryItemUI = GetComponent<InventoryItemUI>();
    }

    public override bool CanBeUsed()
    {
        return _currentState == ItemState.Idle;
    }

    public override TwoStepsUsableItemUI GetItemToHand()
    {
        if (!CanBeUsed()) return null;

        ChangeState(ItemState.OnHand);

        return this;
    }

    public override void ReturnItemToSlot()
    {
        if(_currentState != ItemState.OnHand) return;

        ChangeState(ItemState.Idle);
    }

    public override void UseOnItem(ItemData itemData)
    {
        itemData.Item = itemData.Item.RotatedItem;

        _iventoryItemUI.RemoveFromInventory();
    }

    private void ChangeState(ItemState state)
    {
        _currentState = state;

        switch (state)
        {
            case ItemState.Idle:
                ChangeStateToIdle();
                break;
            case ItemState.OnHand:
                ChangeStateToHand();
                break;
        }
    }

    private void ChangeStateToIdle()
    {
        ChangeAlpha(1f);
    }

    private void ChangeStateToHand()
    {
        ChangeAlpha(0.5f);
    }

    private void ChangeAlpha(float imageAlpha)
    {
        Color color = _itemImage.color;
        color.a = imageAlpha;
        _itemImage.color = color;
    }


    public enum ItemState
    {
        Idle,
        OnHand
    }
}
