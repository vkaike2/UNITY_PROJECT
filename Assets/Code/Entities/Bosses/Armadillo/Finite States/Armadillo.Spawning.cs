using System;
using UnityEngine;

public partial class Armadillo : MonoBehaviour
{
    private class Spawning : ArmadilloBaseBehaviour
    {
        public override Behaviour Behaviour => Armadillo.Behaviour.Spawning;

        public override void OnEnterBehaviour()
        {
            _mainAnimator.PlayAnimation(ArmadilloAnimatorModel.AnimationName.DIG_UP);

            _armadillo.OnFinishSwpawnAnimation.AddListener(FinishAnimation);
        }

        public override void OnExitBehaviour()
        {
            _armadillo.OnFinishSwpawnAnimation.RemoveListener(FinishAnimation);
        }

        public override void Update()
        {
        }

        private void FinishAnimation()
        {
            _armadillo.ChangeBehaviour(Behaviour.Idle);
        }
    }
}