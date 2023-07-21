using System;
using UnityEngine;


public partial class Porcupine : Enemy
{
    private class Patrol : PorcupineBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Patrol;

        private PorcupinePatrolModel _patrolModel = null;
        private EnemyPatrolBehaviour _patrolBehaviour;
        private PorcupineAtkModel _atkModel;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _patrolModel = _porcupine.PatrolModel;
            _atkModel = _porcupine.AtkModel;
            _patrolBehaviour = new EnemyPatrolBehaviour(EnemyPatrolBehaviour.MovementType.Walk, _patrolModel);

            AddEventListeners();

            _patrolBehaviour.Start(enemy);
        }

        public override void OnEnterBehaviour()
        {
            _patrolBehaviour.OnEnterBehaviour();
        }

        public override void OnExitBehaviour()
        {
            _patrolBehaviour.OnExitBehaviour();
        }

        public override void Update()
        {
            _patrolBehaviour.Update();
        }

        private void OnPlayerInRange(GameObject playerGameObject)
        {
            if (!_atkModel.CanAtk) return;

            _porcupine.ChangeBehaviour(Behaviour.Atk);
        }

        private void AddEventListeners()
        {
            _patrolModel.LayerCheckCollider.OnLayerCheckTriggerEnter.AddListener(OnPlayerInRange);
            _patrolModel.OnChangeAnimation.AddListener(OnChangeAnimation);
        }

        private void OnChangeAnimation(EnemyPatrolModel.PossibleAnimations possibleAnimations)
        {
            switch (possibleAnimations)
            {
                case EnemyPatrolModel.PossibleAnimations.Idle:
                    _porcupine.PorculineAnimator.PlayAnimation(PorcupineAnimatorModel.AnimationName.in_Idle);
                    break;
                case EnemyPatrolModel.PossibleAnimations.Move:
                    _porcupine.PorculineAnimator.PlayAnimation(PorcupineAnimatorModel.AnimationName.out_Idle_Move);
                    break;
            }
        }
    }
}
