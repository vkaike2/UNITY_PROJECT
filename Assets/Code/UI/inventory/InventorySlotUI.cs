using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class InventorySlotUI : MonoBehaviour
{
    [Header("VIEW ONLY")]
    [SerializeField]
    private Vector2 _coordinate;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private bool _isFirst;
    [SerializeField]
    private SlotType _type;

    public SlotType Type => _type;
    public Vector2 Coordinate => _coordinate;
    public bool IsFirst => _isFirst;
    public bool HasItem { get; private set; }
    public InventoryItemUI ItemUI { get; private set; }

    private Animator _animator;
    private InventoryUI _inventoryUI;

    private readonly List<Action> _executeSyncronousWithEvent = new List<Action>();
    private readonly List<Action> _executeSyncronous = new List<Action>();

    private void Awake()
    {
        _inventoryUI = GetComponentInParent<InventoryUI>();
        _animator = GetComponent<Animator>();
        HasItem = false;
    }

    private void Update()
    {
        _coordinate = Coordinate;
        ExecuteSyncronousWithEvent();
        ExecuteSyncronous();
    }

    private void ExecuteSyncronousWithEvent()
    {
        if (_executeSyncronousWithEvent.Count == 0) return;

        _executeSyncronousWithEvent.FirstOrDefault().Invoke();
        _executeSyncronousWithEvent.RemoveAt(0);

        if (_executeSyncronousWithEvent.Count == 0)
        {
            switch (_type)
            {
                case SlotType.Inventory:
                    _inventoryUI.UpdatePlayerInventory();
                    break;
                case SlotType.Equip:
                    _inventoryUI.UpdatePlayerEquipInventory();
                    break;
                default:
                    break;
            }

            ;
        }
    }

    private void ExecuteSyncronous()
    {
        if (_executeSyncronous.Count == 0) return;

        _executeSyncronous.FirstOrDefault().Invoke();
        _executeSyncronous.RemoveAt(0);
    }

    public void AddItem(InventoryItemUI item, bool updatePlayer = true)
    {
        if (updatePlayer)
        {
            _executeSyncronousWithEvent.Add(() => AddItemInternal(item));
        }
        else
        {
            _executeSyncronous.Add(() => AddItemInternal(item));
        }
    }

    public void RemoveItem(bool updatePlayer = true)
    {
        if (updatePlayer)
        {
            _executeSyncronousWithEvent.Add(RemoveItemInternal);
        }
        else
        {
            _executeSyncronous.Add(RemoveItemInternal);
        }
    }

    public void ChangeAnimationOnItemOver(bool isItemOver)
    {
        if (HasItem) return;

        if (isItemOver)
        {
            _animator.Play(MyAnimations.ItemOver.ToString());
        }
        else
        {
            _animator.Play(MyAnimations.Idle.ToString());
        }
    }

    public void ClearAnimations()
    {
        if (HasItem) return;

        _animator.Play(MyAnimations.Idle.ToString());
    }

    public void SetCooordinate(Vector2? newCoordinate = null)
    {
        if (_coordinate != Vector2.zero && !_isFirst) return;

        if (newCoordinate == null) _coordinate = Vector2.zero;
        else _coordinate = newCoordinate.Value;
    }

    private void AddItemInternal(InventoryItemUI item)
    {
        HasItem = true;
        ItemUI = item;
        _animator.Play(MyAnimations.WithItem.ToString());
    }

    private void RemoveItemInternal()
    {
        HasItem = false;
        ItemUI = null;
        _animator.Play(MyAnimations.Idle.ToString());
    }

    private enum MyAnimations
    {
        Idle,
        ItemOver,
        WithItem
    }
}
