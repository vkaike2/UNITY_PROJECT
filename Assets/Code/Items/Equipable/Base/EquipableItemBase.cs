using System;
using UnityEngine;

[Serializable]
public abstract class EquipableItemBase
{
    [field: SerializeField]
    public ScriptableItem Item { get; private set; }
    [field: SerializeField]
    public ScriptableItem RotatedItem { get; private set; }

    protected ScriptableItemEvents _events;
    protected GameManager _gameManager;
    protected PlayerStatus _playerStatus;

    public void Initialize(ScriptableItemEvents events, GameManager gameManager)
    {
        _events = events;
        _gameManager = gameManager;
        _events.OnEquipItem.AddListener(OnEquipItemInternal);
        _events.OnUnequipItem.AddListener(OnUnequipItemInternal);
    }

    protected abstract void EquipItem();
    protected abstract void UnequipItem();


    private void OnEquipItemInternal(ScriptableItem item)
    {
        GetPlayerStatus();
        if (!CheckIfItemNameIsEqual(item.name)) return;
        EquipItem();
    }

    private void OnUnequipItemInternal(ScriptableItem item)
    {
        GetPlayerStatus();
        if (!CheckIfItemNameIsEqual(item.name)) return;
        UnequipItem();
    }

    private bool CheckIfItemNameIsEqual(string itemName)
    {
        bool isEqualToItem = Item.name == itemName;
        bool isEqualToRotatedItem = RotatedItem != null && RotatedItem.name == itemName;

        return isEqualToItem || isEqualToRotatedItem;
    }

    protected PlayerStatus GetPlayerStatus()
    {
        if (_playerStatus == null)
        {
            _playerStatus = _gameManager.Player.GetComponent<PlayerStatus>();
        }
        return _playerStatus;
    }
}
