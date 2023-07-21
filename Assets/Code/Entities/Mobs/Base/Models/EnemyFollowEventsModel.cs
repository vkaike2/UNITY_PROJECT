using UnityEngine.Events;
using static EnemyFollowingBehavior;

public class EnemyFollowEventsModel
{
    public OnInteractWithTargetEvent OnInteractWithTarget { get; private set; } = new OnInteractWithTargetEvent();
    public OnChangeAnimationEvent OnChangeAnimation { get; private set; } = new OnChangeAnimationEvent();
    public UnityEvent OnTargetUnreachable { get; private set; } = new UnityEvent();


    public void ResetEvents()
    {
        OnInteractWithTarget.RemoveAllListeners();
        OnChangeAnimation.RemoveAllListeners();
        OnTargetUnreachable.RemoveAllListeners();
    }

    public enum PossibleAnimations
    {
        Idle, Move, Jump, Air
    }
    public class OnChangeAnimationEvent : UnityEvent<PossibleAnimations> { }
}
public class OnInteractWithTargetEvent : UnityEvent<Target> { }