using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySlotUI : MonoBehaviour
{
    [Header("VIEW ONLY")]
    [SerializeField]
    private Vector2 _coordinate;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private SlotType _type;
    [field: SerializeField]
    public bool IsAvalialbe { get; private set; } = true;

    [Header("COMPONENTS")]
    [SerializeField]
    private GameObject _slotOverImage;

    public SlotType Type => _type;
    public Vector2 Coordinate => _coordinate;

    public bool HasItem { get; private set; }
    public InventoryItemUI ItemUI { get; private set; }
    public bool HasError { get; private set; } = false;

    private Animator _slotOverAnimator;
    private Animator _animator;
    private InventoryUI _inventoryUI;

    public bool _isUnlockedByDefault = false;

    private readonly List<Action> _executeSyncronous = new List<Action>();
    private readonly List<Guid> _runningActions = new List<Guid>();

    private void Awake()
    {
        _inventoryUI = GetComponentInParent<InventoryUI>();
        _animator = GetComponent<Animator>();

        HasItem = false;

        _isUnlockedByDefault = IsAvalialbe;
        ToggleAvailability(IsAvalialbe);
    }

    private void Start()
    {
        _slotOverImage.SetActive(true);
        _slotOverAnimator = _slotOverImage.GetComponent<Animator>();
    }

    private void Update()
    {
        _coordinate = Coordinate;
        ExecuteSyncronous();
    }

    public bool CanReceiveANewItem() => IsAvalialbe && !HasItem;

    public Guid AddItem(InventoryItemUI item, bool canBeEquiped = true)
    {
        Guid actionId = GenerateActionId();
        _executeSyncronous.Add(() => AddItemInternal(actionId, item, canBeEquiped));

        return actionId;
    }

    public Guid RemoveItem()
    {
        Guid actionId = GenerateActionId();
        _executeSyncronous.Add(() => RemoveItemInternal(actionId));

        return actionId;
    }

    public bool SyncronousActionIsDone(Guid actionId)
    {
        return !_runningActions.Any(e => e == actionId);
    }

    public void ChangeAnimationOnItemOver(bool isItemOver)
    {
        if (!CanReceiveANewItem()) return;

        if (isItemOver)
        {
            _slotOverAnimator.Play(SlotOverAnimations.ItemOver.ToString());
        }
        else
        {
            _slotOverAnimator.Play(MyAnimations.None.ToString());
        }
    }

    public void ToggleAvailability(bool isAvailable)
    {
        IsAvalialbe = isAvailable;

        if (isAvailable)
        {
            ResetSlotToIdleAnimation();
        }
        else
        {
            _animator.Play(MyAnimations.Disabled.ToString());
        }
    }

    /// <summary>
    ///     Remove the Error if you can
    /// </summary>
    public void FixEquipment()
    {
        Debug.Log("fixing");

        HasError = false;
        ResetSlotToIdleAnimation();
        EquipItemAnimation();
    }

    private Guid GenerateActionId()
    {
        Guid actionId = Guid.NewGuid();
        _runningActions.Add(actionId);
        return actionId;
    }

    private void AddItemInternal(Guid actionId, InventoryItemUI item, bool canBeEquiped = true)
    {
        HasItem = true;
        ItemUI = item;
        if (canBeEquiped)
        {
            HasError = false;
            EquipItemAnimation();
        }
        else
        {
            HasError = true;
            AddItemWithErrorAnimation();
        }

        _runningActions.Remove(actionId);
    }

    private void RemoveItemInternal(Guid actionId)
    {
        HasError = false;
        HasItem = false;
        ItemUI = null;

        ResetSlotToIdleAnimation();

        _slotOverAnimator.Play(MyAnimations.None.ToString());

        _runningActions.Remove(actionId);
    }

    private void ExecuteSyncronous()
    {
        if (_executeSyncronous.Count == 0) return;

        _executeSyncronous.FirstOrDefault().Invoke();
        _executeSyncronous.RemoveAt(0);
    }

    #region UPDATE ANIMATION
    private void EquipItemAnimation()
    {
        switch (_type)
        {
            case SlotType.Equip:

                if (ItemUI.ItemData.Item.IsEquipable)
                {
                    _slotOverAnimator.Play(SlotOverAnimations.WithItemEquiped.ToString());
                }
                else
                {
                    _slotOverAnimator.Play(SlotOverAnimations.WithItem.ToString());
                }

                break;
            case SlotType.Inventory:
                _slotOverAnimator.Play(SlotOverAnimations.WithItem.ToString());
                break;
        }
    }

    private void AddItemWithErrorAnimation()
    {
        _animator.Play(MyAnimations.Error.ToString());
        _slotOverAnimator.Play(SlotOverAnimations.WithItemError.ToString());
    }

    private void ResetSlotToIdleAnimation()
    {
        if (_isUnlockedByDefault)
        {
            _animator.Play(MyAnimations.Idle.ToString());
        }
        else
        {
            _animator.Play(MyAnimations.Unlocked.ToString());
        }
    }
    #endregion

    private enum MyAnimations
    {
        None,
        Idle,
        Disabled,
        Unlocked,
        Error
    }

    private enum SlotOverAnimations
    {
        ItemOver,
        WithItem,
        WithItemEquiped,
        WithItemError
    }
}
