using System;
using UnityEngine;

[Serializable]
public class PistolCrabAnimatorModel : EnemyAnimatorModel
{
    public void PlayAnimation(AnimationName animationName, bool replayIfCurrent = false)
    {
        if (replayIfCurrent)
        {
            base.PlayIt(animationName.ToString());
            return;
        }

        base.PlayAnimation(animationName.ToString());
    }

    public void PlayAnimationHightPriority(MonoBehaviour behaviour, AnimationName animationName, float cdw)
    {
        behaviour.StartCoroutine(SetHightPriorityAnimation(animationName.ToString(), cdw));
    }

    public AnimationName GetCurrentAnimation()
    {
        return Enum.Parse<AnimationName>(base.GetCurrentAnimationString());
    }

    public enum AnimationName
    {
        PistolCrab_Born,
        PistolCrab_Walk,
        PistolCrab_Attack,
        PistolCrab_Die,
        PistolCrab_Idle
    }
}