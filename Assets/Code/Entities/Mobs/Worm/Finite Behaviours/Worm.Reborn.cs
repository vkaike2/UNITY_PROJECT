using System.Collections;
using UnityEngine;

public partial class Worm : Enemy
{

    private class Reborn : WormBaseBehaviour
    {
        public override Worm.Behaviour Behaviour => Worm.Behaviour.Reborn;


        public override void OnEnterBehaviour()
        {
            _worm.WormAnimator.PlayAnimation(WormAnimatorModel.AnimationName.Reborn_Idle);

            _rigidbody2D.velocity = Vector3.zero;
            _rigidbody2D.isKinematic = true;
            _worm.IsBeingTargeted = false;

            _worm.StartCoroutine(WaitToRebornn());
        }

        public override void OnExitBehaviour()
        {
            _rigidbody2D.isKinematic = false;
        }

        public override void Update() { }

        private IEnumerator WaitToRebornn()
        {
            yield return new WaitForSeconds(_worm.RebornModel.ReborningTime);

            _worm.WormAnimator.PlayAnimation(WormAnimatorModel.AnimationName.Reborning);
        }
    }
}
