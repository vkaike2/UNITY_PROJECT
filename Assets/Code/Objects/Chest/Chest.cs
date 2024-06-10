using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MouseInteractable
{
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private Transform _itemSpawnPoint;
    [SerializeField]
    private float _cdwToDeactivate = 2f;
    [SerializeField]
    private ScriptablePossibleDrop _scriptablePossibleDrops;

    [Header("COMPONENTS")]
    [SerializeField]
    private BaseMap _parentMap;
    [SerializeField]
    private Animator _animator;

    public UnityEvent OnInteractWith { get; private set; } = new UnityEvent();

    private const float WAIT_BEFORE_SPAWN = 0.5F;
    private bool _isOpen = false;

    public override void ChangeAnimationOnItemOver(bool isMouseOver)
    {
        if (_isOpen) return;

        if (isMouseOver)
        {
            _animator.Play(MyAnimations.Selected.ToString());
        }
        else
        {
            _animator.Play(MyAnimations.Idle.ToString());
        }
    }

    public void SetPossibleDrops(ScriptablePossibleDrop scriptablePossibleDrop)
    {
        _scriptablePossibleDrops = scriptablePossibleDrop;
    }

    public void ActivateChest()
    {
        _animator.Play(MyAnimations.Idle.ToString());
        _isOpen = false;
    }

    public override void InteractWith(CustomMouse mouse)
    {
        if (_isOpen) return;

        _isOpen = true;
        _animator.Play(MyAnimations.Open.ToString());

        StartCoroutine(SpawnItems());

        OnInteractWith.Invoke();
    }

    private IEnumerator WaitThenDeactivate()
    {
        yield return new WaitForSeconds(_cdwToDeactivate);
        _animator.Play(MyAnimations.FadeOut.ToString());

        yield return new WaitForSeconds(1);
        
        this.gameObject.SetActive(false);
    }

    private IEnumerator SpawnItems()
    {
        yield return new WaitForSeconds(WAIT_BEFORE_SPAWN);

        yield return _scriptablePossibleDrops.PossibleDrop.SpawnEveryItem(_itemSpawnPoint.position, _parentMap.Containers.ItemContainer);

        yield return WaitThenDeactivate();
    }

    private enum MyAnimations
    {
        Idle,
        Selected,
        Open,
        FadeOut
    }
}
