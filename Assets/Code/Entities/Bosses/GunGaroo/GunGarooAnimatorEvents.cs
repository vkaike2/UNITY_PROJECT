using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GunGarooAnimatorEvents : MonoBehaviour
{
    [field: Header("EVENTS")]
    [field: SerializeField]
    public UnityEvent OnJumpFrame { get; private set; }
    [field: SerializeField]
    public UnityEvent OnShootFrame { get; private set; }

    public void JumpFrame() => OnJumpFrame?.Invoke();

    public void ShootFrame() => OnShootFrame?.Invoke();
}
