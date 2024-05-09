
using System;
using System.Collections;
using UnityEngine;

partial class Shark : Enemy
{
    private class Attack : SharkBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Attack;

        public override void OnEnterBehaviour()
        {
            _rigidbody2D.velocity = Vector2.zero;
            _shark.Animator.PlayAnimation(SharkAnimatorModel.AnimationName.Shark_Attack);

            _attackModel.OnAttackFinished.AddListener(OnFinishedAttackAnimation);

            _shark.StartCoroutine(ResetAttackCdw());
        }

        public override void OnExitBehaviour()
        {
            _attackModel.OnAttackFinished.RemoveListener(OnFinishedAttackAnimation);
        }

        public override void Update()
        {
        }

        private void OnFinishedAttackAnimation()
        {
            if (_shark.CurrentBehaviour == Behaviour.Die) return;

            _shark.ChangeBehaviour(Behaviour.Walk);
        }

        private IEnumerator ResetAttackCdw()
        {
            _attackModel.CanAttack = false;
            yield return new WaitForSeconds(_attackModel.AttakCdw);
            _attackModel.CanAttack = true;
        }
    }
}