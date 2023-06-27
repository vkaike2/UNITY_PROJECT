using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PlayerAnimatorModel;
using static UnityEditor.Progress;

public class InventorySlotUI : MonoBehaviour
{
    public bool HasItem { get; private set; }
    public InventoryItemUI ItemUI { get; private set; }

    private Animator _animator;

    private readonly List<Action> _executeSyncronous = new List<Action>();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        HasItem = false;
    }

    private void FixedUpdate()
    {
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

    private enum MyAnimations
    {
        Idle,
        ItemOver,
        WithItem
    }
}
