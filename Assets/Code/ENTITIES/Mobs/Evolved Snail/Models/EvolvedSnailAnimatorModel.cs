using System;
using UnityEngine;

[Serializable]
public class EvolvedSnailAnimatorModel : EnemyAnimatorModel
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
        Evolved_Snail_Idle,
        Evolved_Snail_Creating_Leg,
        Evolved_Snail_Run,
        Evolved_Snail_Attack,
        Evolved_Snail_Jump,
        Evolved_Snail_Air,
        Evolved_Snail_Die
    }
}