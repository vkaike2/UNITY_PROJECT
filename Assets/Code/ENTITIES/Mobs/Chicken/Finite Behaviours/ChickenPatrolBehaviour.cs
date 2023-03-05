using System.Collections;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class ChickenPatrolBehaviour : ChickenFiniteBaseBehaviour
{
    public override Chicken.Behaviour Behaviour => Chicken.Behaviour.Patrol;

    private MySeeker _mySeeker;
    private PatrolService _patrolService;
    private ChickenPatrolBehaviourModel _patrolModel;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _mySeeker = enemy.GetComponent<MySeeker>();

        _patrolService = new PatrolService(
            enemy,
            _chicken.PatrolModel,
            () => _chicken.Animator.PlayAnimation(ChickenAnimatorModel.AnimationName.Idle),
            () => _chicken.Animator.PlayAnimation(ChickenAnimatorModel.AnimationName.Move));

        _patrolModel = _chicken.PatrolModel;
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
            _chicken.ChangeBehaviour(Chicken.Behaviour.FollowingPlayer);
        }
    }

    /// <summary>
    /// It stops automatically when player is in range
    /// </summary>
    private void StartCheckIfPlayerIsInRange()
    {
        if (_gameManager == null || _gameManager.Player == null) return;

        _chicken.StartCoroutine(
            _mySeeker.CheckIfTargetInRange(
                MySeeker.AvailableMovements.Jump,
                _chicken.gameObject,
                _gameManager.Player.gameObject));

    }

}
