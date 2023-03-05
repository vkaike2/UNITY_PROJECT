using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WormPatrolBehaviour : WormFiniteBaseBehaviour
{
    public override Worm.Behaviour Behaviour => Worm.Behaviour.Patrol;

    private PatrolService _patrolService;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _patrolService = new PatrolService(
            enemy, 
            _worm.PatrolModel,
            () => _worm.Animator.PlayAnimation(WormAnimatorModel.AnimationName.Idle),
            () => _worm.Animator.PlayAnimation(WormAnimatorModel.AnimationName.Move));
    }

    public override void OnEnterBehaviour()
    {
        _patrolService.StartPatrolBehaviour();

        StartCheckIfPlayerIsInRange();
    }

    public override void OnExitBehaviour()
    {
        _patrolService.ResetCoroutines();
        _patrolService.DisabelGizmo();  
    }

    public override void Update()
    {
        if (_mySeeker.IsTargetInRange)
        {
            _worm.ChangeBehaviour(Worm.Behaviour.FollowingPlayer);
        }
    }

    /// <summary>
    /// It stops automatically when player is in range
    /// </summary>
    private void StartCheckIfPlayerIsInRange()
    {
        if (_gameManager == null || _gameManager.Player == null) return;

        _worm.StartCoroutine(
            _mySeeker.CheckIfTargetInRange(
                MySeeker.AvailableMovements.Walk,
                _worm.gameObject,
                _gameManager.Player.gameObject));

    }
}
