using System;
using UnityEngine;
using UnityEngine.Events;

public class PorcupineAnimatorEvents : MonoBehaviour
{
    [Header("events")]
    [SerializeField]
    private OnToggleMovement _onCanMove;
    [SerializeField]
    private UnityEvent _onSetInitialBehaviour;
    [SerializeField]
    private UnityEvent _onTriggerAtkProjectiles;
    [SerializeField]
    private UnityEvent _onEndAtk;
    [SerializeField]
    private UnityEvent _onStartAtkJump;

    public UnityEvent OnTriggerAtkProjectile => _onTriggerAtkProjectiles;
    public UnityEvent OnEndAtk => _onEndAtk;
    public UnityEvent OnStartAtkJump => _onStartAtkJump;


    public void SetInitialBehaviour() => _onSetInitialBehaviour.Invoke();
    public void CanMove() => _onCanMove.Invoke(true);
    public void CanNotMove() => _onCanMove.Invoke(false);

    public void TriggerStartAtkJump() => _onStartAtkJump.Invoke();
    public void TriggerAtkProjectiles() => _onTriggerAtkProjectiles.Invoke();
    public void EndAtk() => _onEndAtk.Invoke();

    [Serializable]
    public class OnToggleMovement : UnityEvent<bool> { }
}

