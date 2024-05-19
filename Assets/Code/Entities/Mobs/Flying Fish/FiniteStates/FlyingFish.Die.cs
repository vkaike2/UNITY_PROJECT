using System.Collections;
using UnityEngine;

public partial class FlyingFish : Enemy
{
    private class Die : FlyingFishBaseBehaviour
    {
        public override Behaviour Behaviour => FlyingFish.Behaviour.Die;

        public override void OnEnterBehaviour()
        {
            if (_attackModel.IsUnderTheWater)
            {
                _flyingFish.Animator.PlayAnimation(FlyingFishAnimatorModel.AnimationName.Flying_Fish_Die_Water);
            }
            else
            {
                _flyingFish.Animator.PlayAnimation(FlyingFishAnimatorModel.AnimationName.Flying_Fish_Die_Floor);
            }

            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            _flyingFish.HitBox.gameObject.SetActive(false);

            DropItemIfPossible();

            _flyingFish.StartCoroutine(FadeOutThenDie());
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
            UnityEngine.Color color = _flyingFish.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _flyingFish.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_flyingFish.gameObject);
        }

        private void DropItemIfPossible()
        {
            if (_flyingFish.PossibleDrop == null) return;
            if (_flyingFish.PossibleDrop.ItemPools.Count == 0) return;

            Vector2 dropPosition = new Vector2(_flyingFish.transform.position.x, _flyingFish.transform.position.y + 0.5f);

            _flyingFish.StartCoroutine(_flyingFish.PossibleDrop.SpawnEveryItem(dropPosition, _flyingFish.ItemContainer));
        }
    }
}