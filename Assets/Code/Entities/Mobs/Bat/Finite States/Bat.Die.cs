using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Bat : Enemy
{
    private class Die : BatBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Die;

        public override void OnEnterBehaviour()
        {
            _bat.Animator.PlayAnimation(BatAnimatorModel.AnimationName.Bat_Die);
            _bat.HitBox.gameObject.SetActive(false);
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;

            DropItemIfPossible();

            _bat.StartCoroutine(FadeOutThenDie());
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
            Color color = _bat.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _bat.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_bat.gameObject);
        }

        private void DropItemIfPossible()
        {
            if (_bat.PossibleDrop == null) return;
            if (_bat.PossibleDrop.ItemPools.Count == 0) return;

            Vector2 dropPosition = new Vector2(_bat.transform.position.x, _bat.transform.position.y + 0.5f);

            _bat.StartCoroutine(_bat.PossibleDrop.SpawnEveryItem(dropPosition, _bat.ItemContainer));
        }
    }
}
