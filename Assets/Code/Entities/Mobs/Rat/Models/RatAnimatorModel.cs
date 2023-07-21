using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RatAnimatorModel : EnemyAnimatorModel
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
        Rat_Born,
        Rat_Idle,
        Rat_Moving,
        Rat_Air,
        Rat_Die
    }
}
