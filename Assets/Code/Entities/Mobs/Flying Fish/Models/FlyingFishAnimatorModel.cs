using System;
using UnityEngine;

[Serializable]
public class FlyingFishAnimatorModel : EnemyAnimatorModel
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
        Flying_Fish_Born,
        Flying_Fish_Floor,
        Flying_Fish_Die_Floor,
        Flying_Fish_Die_Water,
        Flying_Fish_Idle,
        Flying_Fish_Walk
    }
}