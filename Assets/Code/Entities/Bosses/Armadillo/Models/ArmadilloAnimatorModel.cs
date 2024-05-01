

using System;
using UnityEngine;

[Serializable]
public class ArmadilloAnimatorModel : EnemyAnimatorModel
{
    public void PlayAnimation(AnimationName animationName)
    {
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
        DIG_UP,
        IDLE,
        RUN,
        HIT_WALL,
        STUNNED,
        SHOCKWAVE,
        INTO_BALL,
        DEAD,
        DIG_DOWN
    }
}