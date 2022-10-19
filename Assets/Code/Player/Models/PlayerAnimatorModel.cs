using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimatorModel
{
    [SerializeField]
    private Animator _animator;

    public Animator Animator => _animator;

    public void PlayAnimation(Animations animation)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(animation.ToString())) return;

        _animator.Play(animation.ToString());
    }

    public enum Animations
    {
        Idle,
        Move,
        Jump,
        Falling
    }
}
