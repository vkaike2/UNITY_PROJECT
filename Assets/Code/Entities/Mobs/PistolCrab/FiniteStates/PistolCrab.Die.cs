using System.Collections;
using UnityEngine;

public partial class PistolCrab : Enemy
{
    private class Die : PistolCrabBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Die;

        public override void OnEnterBehaviour()
        {
            _pistolCrab.Animator.PlayAnimation(PistolCrabAnimatorModel.AnimationName.PistolCrab_Die);

            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            _pistolCrab.HitBox.gameObject.SetActive(false);

            DropItemIfPossible();

            _pistolCrab.StartCoroutine(FadeOutThenDie());
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
            UnityEngine.Color color = _pistolCrab.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _pistolCrab.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_pistolCrab.gameObject);
        }

        private void DropItemIfPossible()
        {
            if (_pistolCrab.PossibleDrop == null) return;
            if (_pistolCrab.PossibleDrop.ItemPools.Count == 0) return;

            Vector2 dropPosition = new Vector2(_pistolCrab.transform.position.x, _pistolCrab.transform.position.y + 0.5f);

            _pistolCrab.StartCoroutine(_pistolCrab.PossibleDrop.SpawnEveryItem(dropPosition, _pistolCrab.ItemContainer));
        }
    }
}