using System.Collections;
using UnityEngine;

public partial class Scorpion : Enemy
{
    private class Die : ScorpionBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Die;

        public override void OnEnterBehaviour()
        {
            _scorpion.Animator.PlayAnimation(ScorpionAnimatorModel.AnimationName.Scorpion_Die);

            _scorpion.HitBox.gameObject.SetActive(false);
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;

            DropItemIfPossible();

            _scorpion.StartCoroutine(FadeOutThenDie());
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
            UnityEngine.Color color = _scorpion.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _scorpion.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_scorpion.gameObject);
        }

        private void DropItemIfPossible()
        {
            if (_scorpion.PossibleDrop == null) return;
            if (_scorpion.PossibleDrop.ItemPools.Count == 0) return;

            Vector2 dropPosition = new Vector2(_scorpion.transform.position.x, _scorpion.transform.position.y + 0.5f);

            _scorpion.StartCoroutine(_scorpion.PossibleDrop.SpawnEveryItem(dropPosition, _scorpion.ItemContainer));
        }
    }
}