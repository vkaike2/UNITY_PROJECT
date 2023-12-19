using System.Collections;
using UnityEngine;


public partial class Snail : Enemy
{

    private class Die : SnailBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Die;

        public override void OnEnterBehaviour()
        {
            _snail.Animator.PlayAnimation(SnailAnimatorModel.AnimationName.Snail_Die);
            _snail.HitBox.gameObject.SetActive(false);
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;

            DropItemIfPossible();

            _snail.StartCoroutine(FadeOutThenDie());
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
            Color color = _snail.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _snail.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_snail.gameObject);
        }

        private void DropItemIfPossible()
        {
            if (_snail.PossibleDrop == null) return;
            if (_snail.PossibleDrop.ItemPools.Count == 0) return;

            Vector2 dropPosition = new Vector2(_snail.transform.position.x, _snail.transform.position.y + 0.5f);

            _snail.StartCoroutine(_snail.PossibleDrop.SpawnEveryItem(dropPosition, _snail.ItemContainer));
        }
    }
}
