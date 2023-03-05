using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAnimatorModel 
{
    [SerializeField]
    private Animator _animator;

    public Animator Animator => _animator;

    private string _lowPriorityAnimationName;
    private bool _IsPlayingHightPriority = false;


    public void PlayAnimation(string animationName)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) return;

        _lowPriorityAnimationName = animationName;

        if (_IsPlayingHightPriority) return;

        _animator.Play(animationName);
    }


    protected IEnumerator SetHightPriorityAnimation(string animationName, float cdw)
    {
        _IsPlayingHightPriority = true;
        _animator.Play(animationName);

        yield return new WaitForSeconds(cdw);

        _IsPlayingHightPriority = false;
        _animator.Play(_lowPriorityAnimationName);
    }


    public string GetCurrentAnimationString()
    {
        AnimatorClipInfo[] info = _animator.GetCurrentAnimatorClipInfo(0);
        return info[0].clip.name;
    }
}
