using System.Collections;
using UnityEngine;

public partial class Shark : Enemy
{
    private class Die : SharkBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Die;

        public override void OnEnterBehaviour()
        {
            _shark.Animator.PlayAnimation(SharkAnimatorModel.AnimationName.Shark_Die);

            _rigidbody2D.velocity = Vector2.zero;
             _rigidbody2D.bodyType = RigidbodyType2D.Static;
            _shark.HitBox.gameObject.SetActive(false);
            _shark.AttackModel.AttackHitboxCollider.gameObject.SetActive(false);

            DropItemIfPossible();

            _shark.StartCoroutine(FadeOutThenDie());
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
            UnityEngine.Color color = _shark.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _shark.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_shark.gameObject);
        }

        private void DropItemIfPossible()
        {
            if (_shark.PossibleDrop == null) return;
            if (_shark.PossibleDrop.ItemPools.Count == 0) return;

            Vector2 dropPosition = new Vector2(_shark.transform.position.x, _shark.transform.position.y + 0.5f);

            _shark.StartCoroutine(_shark.PossibleDrop.SpawnEveryItem(dropPosition, _shark.ItemContainer));
        }
    }
}