using System;
using UnityEngine;
using static ArmadilloAnimatorModel;

[Serializable]
public class SharkAnimatorModel : EnemyAnimatorModel
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
        Shark_Born,
        Shark_Walk,
        Shark_Die,
        Shark_Attack
    }
}