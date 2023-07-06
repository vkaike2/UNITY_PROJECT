using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour
{
    [Header("VIEW ONLY")]
    [SerializeField]
    private Vector2 _coordinate;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private bool _isFirst;

    public Vector2 Coordinate => _coordinate;
    public bool IsFirst => _isFirst;
    public bool HasItem { get; private set; }
    public InventoryItemUI ItemUI { get; private set; }

    private Animator _animator;

    private readonly List<Action> _executeSyncronous = new List<Action>();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        HasItem = false;
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        _coordinate = Coordinate;

        if (_executeSyncronous.Count == 0) return;

        _executeSyncronous.FirstOrDefault().Invoke();
        _executeSyncronous.RemoveAt(0);
    }

    public void AddItem(InventoryItemUI item)
    {
        _executeSyncronous.Add(() =>
        {
            HasItem = true;
            ItemUI = item;
            _animator.Play(MyAnimations.WithItem.ToString());
        });
    }

    public void RemoveItem()
    {
        _executeSyncronous.Add(() =>
        {
            HasItem = false;
            ItemUI = null;
            _animator.Play(MyAnimations.Idle.ToString());
        });
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

    private enum MyAnimations
    {
        Idle,
        ItemOver,
        WithItem
    }
}
