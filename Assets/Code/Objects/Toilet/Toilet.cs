using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Toilet : MouseInteractable
{
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private State _state;

    [Header("COMPONENTS")]
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private PlayerInRangeCheck _playerInRangeCheck;


    private const float CDW_WAIT_ANIMATION_TRANSITION = 1f;

    public OnToggleToiletEvent OnToggleToiletEvent { get; private set; }

    private void Awake()
    {
        OnToggleToiletEvent = new OnToggleToiletEvent();

        if (_state == State.Disabled)
        {
            _animator.Play(MyAnimations.Disabled.ToString());
        }
    }

    public override void ChangeAnimationOnItemOver(bool isMouseOver)
    {
        if (!_playerInRangeCheck.PlayerIsInRange)
        {
            ManageMouseOver(false);
            return;
        }

        ManageMouseOver(isMouseOver);
    }

    private void ManageMouseOver(bool isMouseOver)
    {
        if (_state == State.Closed)
        {
            MouseOveOnToiletClosed(isMouseOver);
        }

        if (_state == State.Open)
        {
            MouseOveOnToiletOpen(isMouseOver);
        }
    }

    public override void InteractWith(CustomMouse mouse)
    {
        if (!_playerInRangeCheck.PlayerIsInRange) return;

        if (_state == State.Closed)
        {
            _animator.Play(MyAnimations.Open.ToString());
            StartCoroutine(WaitThenChangeState(State.Open));
            return;
        }

        if (_state == State.Open)
        {

            _animator.Play(MyAnimations.Closing.ToString());
            StartCoroutine(WaitThenChangeState(State.Closed));
            return;
        }

    }

    public void OpenToilet()
    {
        _state = State.Open;
        _animator.Play(MyAnimations.Opened.ToString());
        OnToggleToiletEvent.Invoke(State.Open);
    }

    public void DisableIt()
    {
        _state = State.Disabled;
        _animator.Play(MyAnimations.Disabled.ToString());
        OnToggleToiletEvent.Invoke(State.Disabled);
    }

    private void MouseOveOnToiletClosed(bool isMouseOver)
    {
        if (!PlayerIsInRange())
        {
            _animator.Play(MyAnimations.Closed.ToString());
            return;
        }

        if (isMouseOver)
        {
            _animator.Play(MyAnimations.ClosedSelected.ToString());
        }
        else
        {
            _animator.Play(MyAnimations.Closed.ToString());
        }
    }

    private void MouseOveOnToiletOpen(bool isMouseOver)
    {
        if (!PlayerIsInRange())
        {
            _animator.Play(MyAnimations.Opened.ToString());
            return;
        }

        if (isMouseOver)
        {
            _animator.Play(MyAnimations.OpenedSelected.ToString());
        }
        else
        {
            _animator.Play(MyAnimations.Opened.ToString());
        }
    }

    private IEnumerator WaitThenChangeState(State nextState)
    {
        _state = State.Transition;
        yield return new WaitForSeconds(CDW_WAIT_ANIMATION_TRANSITION);
        _state = nextState;
        OnToggleToiletEvent.Invoke(nextState);
    }

    private enum MyAnimations
    {
        Closed,
        ClosedSelected,
        Open,
        Opened,
        OpenedSelected,
        Closing,
        Disabled
    }

    public enum State
    {
        Closed,
        Open,
        Disabled,
        Transition
    }
}

[Serializable]
public class OnToggleToiletEvent : UnityEvent<Toilet.State> { }
