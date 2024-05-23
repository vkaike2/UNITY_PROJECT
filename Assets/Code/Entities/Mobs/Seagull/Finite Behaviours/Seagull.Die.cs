using System.Collections;
using UnityEngine;

public partial class Seagull : Enemy
{
    private class Die : SeagullBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Die;

        public override void OnEnterBehaviour()
        {
            if (_seagull.IsFlying)
            {
                _seagull.Animator.PlayAnimation(SeagullAnimatorModel.AnimationName.Seagull_Die_Air);
            }
            else
            {
                _seagull.Animator.PlayAnimation(SeagullAnimatorModel.AnimationName.Seagull_Die_Ground);
            }

            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            _seagull.HitBox.gameObject.SetActive(false);

            DropItemIfPossible();

            _seagull.StartCoroutine(FadeOutThenDie());
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
            UnityEngine.Color color = _seagull.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _seagull.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_seagull.gameObject);
        }

        private void DropItemIfPossible()
        {
            if (_seagull.PossibleDrop == null) return;
            if (_seagull.PossibleDrop.ItemPools.Count == 0) return;

            Vector2 dropPosition = new Vector2(_seagull.transform.position.x, _seagull.transform.position.y + 0.5f);

            _seagull.StartCoroutine(_seagull.PossibleDrop.SpawnEveryItem(dropPosition, _seagull.ItemContainer));
        }
    }
}