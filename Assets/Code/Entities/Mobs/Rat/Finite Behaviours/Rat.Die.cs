using System.Collections;
using UnityEngine;


public partial class Rat : Enemy
{
    private class Die : RatBaseBehaviour
    {
        public override Behaviour Behaviour => Rat.Behaviour.Die;

        public override void OnEnterBehaviour()
        {
            _rat.RatAnimator.PlayAnimation(RatAnimatorModel.AnimationName.Rat_Die);
            _rat.HitBox.gameObject.SetActive(false);
            _rigidbody2D.velocity = Vector3.zero;
            _rigidbody2D.isKinematic = true;
            _rat.StartCoroutine(FadeOutThenDie());
        }

        public override void OnExitBehaviour()
        {
        }

        public override void Update()
        {
        }
        private IEnumerator FadeOutThenDie(float fadeOutTime = 2f)
        {
            float cdw = fadeOutTime;
            Color color = _rat.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _rat.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_rat.gameObject);
        }
    }
}
