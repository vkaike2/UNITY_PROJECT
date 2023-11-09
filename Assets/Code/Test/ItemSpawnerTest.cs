using System;
using System.Collections;
using TMPro;
using UnityEngine;


public class ItemSpawnerTest : MouseInteractable
{
    [Header("CONFIGURATION")]
    [SerializeField]
    private ScriptableItem _item;

    [Header("COMPONENTS")]
    [SerializeField]
    private TMP_Text _itemLabel;

    private void OnValidate()
    {
        if (_item == null) return;

        _itemLabel.text = _item.Identity.Name.ToUpper();
    }

    public override void ChangeAnimationOnItemOver(bool isMouseOver)
    {
    }

    public override void InteractWith(CustomMouse mouse)
    {
        ItemData itemData = new ItemData(Guid.NewGuid(), _item);
        ItemDrop itemDrop = Instantiate(itemData.Item.ItemDrop, this.transform);
        itemDrop.transform.parent = null;
        itemDrop.ItemData = itemData;
        itemDrop.DropItem();
    }
}

