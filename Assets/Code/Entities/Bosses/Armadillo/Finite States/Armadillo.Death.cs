using UnityEngine;

public partial class Armadillo : MonoBehaviour
{
    private class Death : ArmadilloBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Death;

        public override void OnEnterBehaviour()
        {
            _armadillo.StopAllCoroutines();
            _armadillo.Colliders.DeactivateEveryHitbox();
            
            _rigidBody2D.velocity = new Vector2(0, -5);
            _mainAnimator.PlayAnimation(ArmadilloAnimatorModel.AnimationName.DEAD);
        }

        public override void OnExitBehaviour()
        {
        }

        public override void Update()
        {
        }
    }
}