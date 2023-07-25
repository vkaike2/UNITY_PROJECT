using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Chicken : Enemy
{
    public class Die : ChickenBaseBehaviour
    {
        public override Chicken.Behaviour Behaviour => Chicken.Behaviour.Die;

        public override void OnEnterBehaviour()
        {
            _chicken.ChickenAnimator.PlayAnimation(ChickenAnimatorModel.AnimationName.Die);
            _chicken.HitBox.gameObject.SetActive(false);
            _rigidbody2D.velocity = Vector3.zero;
            _rigidbody2D.isKinematic = true;
            _chicken.StartCoroutine(FadeOutThenDie());
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
            Color color = _chicken.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _chicken.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_chicken.gameObject);
        }
    }
}