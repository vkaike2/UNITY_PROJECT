
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public partial class Seagull : Enemy
{
    private class Ground : SeagullBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Ground;

        public override void OnEnterBehaviour()
        {
            _seagull.IsFlying = false;
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;

            _seagull.Animator.PlayAnimation(SeagullAnimatorModel.AnimationName.Seagull_Ground);

            foreach (var collider in _seagull.MainColliders)
            {
                collider.enabled = true;
            }

            _seagull.StartCoroutine(WaitToFlyAgain());

            _flyModel.OnFlappingWings.AddListener(OnFlapWings);
        }

        public override void OnExitBehaviour()
        {
            _flyModel.OnFlappingWings.RemoveListener(OnFlapWings);
            foreach (var collider in _seagull.MainColliders)
            {
                collider.enabled = false;
            }
        }

        public override void Update() { }

        private void OnFlapWings()
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _flyModel.VeriticalVelocityToGoUp);
        }

        private IEnumerator WaitToFlyAgain()
        {
            yield return new WaitForSeconds(_groundModel.GroundDuration);

            _seagull.StartCoroutine(GoBackUpAgain());
        }

        private IEnumerator GoBackUpAgain()
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _seagull.Animator.PlayAnimation(SeagullAnimatorModel.AnimationName.Seagull_Fly);
            _seagull.IsFlying = true;
            yield return new WaitUntil(() => _seagull.transform.position.y >= _flyModel.InitialVerticalPosition);
            _seagull.ChangeBehaviour(Behaviour.Fly);
        }
    }
}