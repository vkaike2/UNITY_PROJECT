using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ScriptableItemEvents;

[Serializable]
public abstract class UsableItemBase
{
    [field: SerializeField]
    public ScriptableItem Item { get; private set; }
    [field: SerializeField]
    public ScriptableItem RotatedItem { get; private set; }

    protected ScriptableItemEvents _events;
    protected GameManager _gameManager;
    protected PlayerDamageReceiver _playerDamageReceiver;
    protected PlayerInventory _playerInventory;


    public void Initialize(ScriptableItemEvents events, GameManager gameManager)
    {
        _events = events;
        _gameManager = gameManager;

        _events.OnUseItem.AddListener(OnUseItemInternal);
    }

    protected abstract void UseItem();

    private void OnUseItemInternal(ScriptableItem item)
    {
        GetPlayerComponents();
        if (!CheckIfItemNameIsEqual(item.name)) return;
        UseItem();
    }

    private bool CheckIfItemNameIsEqual(string itemName)
    {
        bool isEqualToItem = Item.name == itemName;
        bool isEqualToRotatedItem = RotatedItem != null && RotatedItem.name == itemName;

        return isEqualToItem || isEqualToRotatedItem;
    }

    protected void GetPlayerComponents()
    {
        if(_playerInventory == null)
        {
            _playerInventory = _gameManager.PlayerInventory;
        }

        if (_playerDamageReceiver == null)
        {
            _playerDamageReceiver = _gameManager.Player.GetComponent<PlayerDamageReceiver>();
        }
    }
}
