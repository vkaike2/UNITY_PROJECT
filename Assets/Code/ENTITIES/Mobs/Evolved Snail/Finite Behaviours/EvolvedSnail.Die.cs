
using System.Collections;
using UnityEngine;

public partial class EvolvedSnail : Enemy
{
    private class Die : EvolvedSnailBaseBhaviour
    {
        public override Behaviour Behaviour => Behaviour.Die;

        public override void OnEnterBehaviour()
        {
            _evolvedSnail.AttackModel.CanAttack = false;

            _evolvedSnail.Animator.PlayAnimation(EvolvedSnailAnimatorModel.AnimationName.Evolved_Snail_Die);

            _evolvedSnail.HitBox.gameObject.SetActive(false);
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;

            DropItemIfPossible();

            _evolvedSnail.StartCoroutine(FadeOutThenDie());
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
            Color color = _evolvedSnail.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _evolvedSnail.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_evolvedSnail.gameObject);
        }

        private void DropItemIfPossible()
        {
            if (_evolvedSnail.PossibleDrop == null) return;
            if (_evolvedSnail.PossibleDrop.ItemPools.Count == 0) return;

            Vector2 dropPosition = new Vector2(_evolvedSnail.transform.position.x, _evolvedSnail.transform.position.y + 0.5f);

            _evolvedSnail.StartCoroutine(_evolvedSnail.PossibleDrop.SpawnEveryItem(dropPosition, _evolvedSnail.ItemContainer));
        }
    }
}