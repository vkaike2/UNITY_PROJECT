using System.Collections;
using UnityEngine;

public partial class Scorpion : Enemy
{
    private class Idle : ScorpionBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Idle;

        private Coroutine _waitToWalk;

        public override void OnEnterBehaviour()
        {
            _scorpion.Animator.PlayAnimation(ScorpionAnimatorModel.AnimationName.Scorpion_Idle);
            _waitToWalk = _scorpion.StartCoroutine(WaitCooldownToWalk());
        }

        public override void OnExitBehaviour()
        {
            _scorpion.StopCoroutine(_waitToWalk);
        }

        public override void Update()
        {
        }

        private IEnumerator WaitCooldownToWalk()
        {
            yield return new WaitForSeconds(_idleModel.CdwToWalk);
            _scorpion.ChangeBehaviour(Behaviour.Walk);
        }
    }
}