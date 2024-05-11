using System.Collections;
using UnityEngine;

partial class StarFish : Enemy
{
    private class Idle : StarFishBaseBehaviour
    {
        public override Behaviour Behaviour => StarFish.Behaviour.Idle;

        public override void OnEnterBehaviour()
        {
            _starFish.Animator.PlayAnimation(StarFishAnimatorModel.AnimationName.Star_Fish_Idle);
            _starFish.StartCoroutine(WaitIdleDuration());
        }

        public override void OnExitBehaviour()
        {
        }

        public override void Update()
        {
        }

        private IEnumerator WaitIdleDuration()
        {
            _starFish.CdwIndicationUI.StartCdw(_idleModel.Duration);

            yield return new WaitForSeconds(_idleModel.Duration);
            if (_starFish.CurrentBehaviour != StarFish.Behaviour.Die)
            {
                _starFish.ChangeBehaviour(StarFish.Behaviour.Attack);
            }

        }
    }
}