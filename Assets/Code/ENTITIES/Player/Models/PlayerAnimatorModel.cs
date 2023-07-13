using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class PlayerAnimatorModel
{
    [SerializeField]
    private Animator _animator;

    public Animator Animator => _animator;

    private Animation _lowPriorityAnimation;
    private bool _IsPlayingHightPriority = false;

    public void PlayAnimation(Animation animation)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(animation.ToString())) return;

        _lowPriorityAnimation = animation;
        
        if (_IsPlayingHightPriority) return;

        _animator.Play(animation.ToString());
    }

    public void PlayAnimationHightPriority(MonoBehaviour behaviour, Animation animation, float cdw)
    {
        behaviour.StartCoroutine(SetHightPriorityAnimation(animation, cdw));
    }

    IEnumerator SetHightPriorityAnimation(Animation animation, float cdw)
    {
        _IsPlayingHightPriority = true;
        _animator.Play(animation.ToString());

        yield return new WaitForSeconds(cdw);
        
        _IsPlayingHightPriority = false;
        _animator.Play(_lowPriorityAnimation.ToString());
    }


    public Animation GetCurrentAnimation()
    {
        AnimatorClipInfo[] info = _animator.GetCurrentAnimatorClipInfo(0);
        return Enum.Parse<Animation>(info[0].clip.name);
    }

    public enum Animation
    {
        Idle,
        Move,
        Jump,
        Falling,
        Fart,
        Pooping
    }
}
