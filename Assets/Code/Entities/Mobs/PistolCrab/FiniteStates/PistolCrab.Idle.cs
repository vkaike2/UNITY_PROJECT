using System;
using System.Collections;
using UnityEngine;

public partial class PistolCrab : Enemy
{
    private class Idle : PistolCrabBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Idle;

        public override void OnEnterBehaviour()
        {
            _pistolCrab.Animator.PlayAnimation(PistolCrabAnimatorModel.AnimationName.PistolCrab_Idle);
            _pistolCrab.StartCoroutine(WaitIdleDuration());
        }

        public override void OnExitBehaviour()
        {
        }

        public override void Update()
        {
        }

        private IEnumerator WaitIdleDuration()
        {
            yield return new WaitForSeconds(_pistolCrab.IdleModel.IdleDuration);

            if (CanAttackPlayer())
            {
                _pistolCrab.ChangeBehaviour(Behaviour.Attack);
            }
            else
            {
                _pistolCrab.ChangeBehaviour(Behaviour.Walk);
            }
        }
    }
}