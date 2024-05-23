using System;
using UnityEngine;

[Serializable]
public class SeagullAnimatorModel : EnemyAnimatorModel
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
        Seagull_Born,
        Seagull_Fly,
        Seagull_Die_Air,
        Seagull_Die_Ground,
        Seagull_Ground
    }
}