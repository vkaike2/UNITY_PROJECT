using System;
using UnityEngine;

[Serializable]
public class GunGarooAnimatorModel : EnemyAnimatorModel
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
        Spawning,
        Spawning_Ground,
        Idle,
        Jump_Start,
        Jump_Air,
        Jump_Air_Down,
        Shoot,
        SuperJump_Start,
        SuperJump_Air_Up,
        SuperJump_Air_Down,
        SuperJump_Landing,
        Death_Start,
        Death
    }
}
