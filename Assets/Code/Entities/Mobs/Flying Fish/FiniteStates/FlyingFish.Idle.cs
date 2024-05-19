using System.Collections;
using UnityEngine;

public partial class FlyingFish : Enemy
{
    private class Idle : FlyingFishBaseBehaviour
    {
        public override Behaviour Behaviour => FlyingFish.Behaviour.Idle;

        public override void OnEnterBehaviour()
        {
            CalculateNextMovement();
        }

        public override void OnExitBehaviour()
        {
        }

        public override void Update()
        {
        }

        private void CalculateNextMovement()
        {
            if (_attackModel.IsReadyToAttack)
            {
                _flyingFish.ChangeBehaviour(Behaviour.Attack);
                return;
            }

            _flyingFish.StartCoroutine(WaitToWalk());
        }

        private IEnumerator WaitToWalk()
        {
            _flyingFish.Animator.PlayAnimation(FlyingFishAnimatorModel.AnimationName.Flying_Fish_Idle);
            yield return new WaitForSeconds(_idleModel.CdwToWalk);
            _flyingFish.ChangeBehaviour(FlyingFish.Behaviour.Walk);
        }
    }
}