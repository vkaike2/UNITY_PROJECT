using System.Collections;
using UnityEngine;

partial class StarFish : Enemy
{
    private class Die : StarFishBaseBehaviour
    {
        public override Behaviour Behaviour => StarFish.Behaviour.Die;

        public override void OnEnterBehaviour()
        {
            _starFish.Animator.PlayAnimation(StarFishAnimatorModel.AnimationName.Star_Fish_Die);

            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            _starFish.HitBox.gameObject.SetActive(false);

            DropItemIfPossible();

            _starFish.StartCoroutine(FadeOutThenDie());
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
            UnityEngine.Color color = _starFish.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _starFish.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_starFish.gameObject);
        }

        private void DropItemIfPossible()
        {
            if (_starFish.PossibleDrop == null) return;
            if (_starFish.PossibleDrop.ItemPools.Count == 0) return;

            Vector2 dropPosition = new Vector2(_starFish.transform.position.x, _starFish.transform.position.y + 0.5f);

            _starFish.StartCoroutine(_starFish.PossibleDrop.SpawnEveryItem(dropPosition, _starFish.ItemContainer));
        }
    }
}