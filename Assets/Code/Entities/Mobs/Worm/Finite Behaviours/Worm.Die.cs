using System;
using System.Collections;
using UnityEngine;


public partial class Worm : Enemy
{
    private class Die : WormBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Die;

        public override void OnEnterBehaviour()
        {
            _worm.WormAnimator.PlayAnimation(WormAnimatorModel.AnimationName.Die);
            _worm.HitBox.gameObject.SetActive(false);
            _rigidbody2D.velocity = Vector3.zero;
            _rigidbody2D.isKinematic = true;

            DropItemIfPossible();

            _worm.StartCoroutine(FadeOutThenDie());
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
            Color color = _worm.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _worm.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_worm.gameObject);
        }
    
        private void DropItemIfPossible()
        {
            if (_worm.PossibleDrop == null) return;
            if(_worm.PossibleDrop.ItemPools.Count == 0) return;

            Vector2 dropPosition = new Vector2(_worm.transform.position.x, _worm.transform.position.y + 0.5f);

            _worm.StartCoroutine(_worm.PossibleDrop.SpawnEveryItem(dropPosition, _worm.ItemContainer));
        }
    }
}
