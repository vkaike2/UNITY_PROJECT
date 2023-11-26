using System.Collections;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;


public partial class Bird : Enemy
{
    private class Die : BirdBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Die;

        public override void OnEnterBehaviour()
        {
            _bird.BirdAnimator.PlayAnimation(BridAnimatorModel.AnimationName.Bird_Die);
            _bird.HitBox.gameObject.SetActive(false);
            _rigidbody2D.velocity = Vector3.zero;
            _rigidbody2D.isKinematic = true;

            DropItemIfPossible();

            _bird.StartCoroutine(FadeOutThenDie());
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
            Color color = _bird.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _bird.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_bird.gameObject);
        }

        private void DropItemIfPossible()
        {
            if (_bird.PossibleDrop == null) return;
            if (_bird.PossibleDrop.ItemPools.Count == 0) return;

            Vector2 dropPosition = new Vector2(_bird.transform.position.x, _bird.transform.position.y + 0.5f);

            _bird.StartCoroutine(_bird.PossibleDrop.SpawnEveryItem(_bird.transform.position, _bird.ItemContainer));
        }

    }
}
