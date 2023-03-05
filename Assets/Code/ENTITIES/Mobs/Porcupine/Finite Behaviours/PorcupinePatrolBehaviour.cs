using UnityEngine;

public class PorcupinePatrolBehaviour : PorcupineFiniteBaseBehaviour
{
    public override Porcupine.Behaviour Behaviour => Porcupine.Behaviour.Patrol;

    private PatrolService _patrolService;
    private PorcupinePatrolBehaviourModel _patrolModel;
    private PorcupineAtkBehaviourModel _atkModel;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        _patrolService = new PatrolService(
            enemy,
            _porcupine.PatrolModel,
            () => _porcupine.Animator.PlayAnimation(PorcupineAnimatorModel.AnimationName.in_Idle),
            () => _porcupine.Animator.PlayAnimation(PorcupineAnimatorModel.AnimationName.out_Idle_Move));

        _patrolModel = _porcupine.PatrolModel;
        _atkModel = _porcupine.AtkModel;
        _patrolModel.LayerCheckCollider.OnLayerCheckTriggerEnter.AddListener(OnPlayerInRange);
    }

    public override void OnEnterBehaviour()
    {
        _patrolService.StartPatrolBehaviour();
    }

    public override void OnExitBehaviour()
    {
        _patrolService.ResetCoroutines();
        _patrolService.DisabelGizmo();
    }

    public override void Update() { }

    private void OnPlayerInRange(GameObject playerGameObject)
    {
        if (!_atkModel.CanAtk) return;

        _porcupine.ChangeBehaviour(Porcupine.Behaviour.Atk);
    }
}
