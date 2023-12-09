using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private readonly List<Action> _executeSynchronous = new List<Action>();

    private Coroutine _syncExecution;

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

        _executeSynchronous.Add(() => EquipItem());
        TryToStartSyncExecution();
    }

    private void OnUnequipItemInternal(ScriptableItem item)
    {
        GetPlayerStatus();
        if (!CheckIfItemNameIsEqual(item.name)) return;

        _executeSynchronous.Add(() => UnequipItem());
        TryToStartSyncExecution();
    }

    private bool CheckIfItemNameIsEqual(string itemName)
    {
        bool isEqualToItem = Item.name == itemName;
        bool isEqualToRotatedItem = RotatedItem != null && RotatedItem.name == itemName;

        return isEqualToItem || isEqualToRotatedItem;
    }

    private PlayerStatus GetPlayerStatus()
    {
        if (_playerStatus == null)
        {
            _playerStatus = _gameManager.Player.GetComponent<PlayerStatus>();
        }
        return _playerStatus;
    }

    private void TryToStartSyncExecution()
    {
        if (_syncExecution != null) return;

        _syncExecution = _gameManager.StartCoroutine(RunSynchronousInternalEvents());
    }

    private IEnumerator RunSynchronousInternalEvents()
    {
        _executeSynchronous.FirstOrDefault().Invoke();
        _executeSynchronous.RemoveAt(0);

        yield return new WaitForFixedUpdate();

        if(_executeSynchronous.Count > 0)
        {
            _syncExecution = _gameManager.StartCoroutine(RunSynchronousInternalEvents());
        }
        else
        {
            _syncExecution = null;
        }
    } 
}
