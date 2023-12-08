using System;
using System.Collections;
using UnityEngine;


public abstract class TwoStepsUsableItemUI : MonoBehaviour
{
    public abstract Sprite DraggableSprite { get; }

    public abstract bool CanBeUsed();

    public abstract TwoStepsUsableItemUI GetItemToHand();

    public abstract void ReturnItemToSlot();

    public abstract void UseOnItem(ItemData itemData);
}
