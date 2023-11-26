using System.Collections;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public partial class Porcupine : Enemy
{
    private class Die : PorcupineBaseBehaviour
    {
        public override Porcupine.Behaviour Behaviour => Porcupine.Behaviour.Die;

        public override void OnEnterBehaviour()
        {
            _porcupine.AtkModel.CanAtk = false;


            _porcupine.PorculineAnimator.PlayAnimation(PorcupineAnimatorModel.AnimationName.Die);
            _porcupine.HitBox.gameObject.SetActive(false);
            _rigidbody2D.velocity = Vector3.zero;
            _rigidbody2D.isKinematic = true;

            DropItemIfPossible();

            _porcupine.StartCoroutine(FadeOutThenDie());
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
            Color color = _porcupine.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _porcupine.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_porcupine.gameObject);
        }

        private void DropItemIfPossible()
        {
            if (_porcupine.PossibleDrop == null) return;
            if (_porcupine.PossibleDrop.ItemPools.Count == 0) return;

            Vector2 dropPosition = new Vector2(_porcupine.transform.position.x, _porcupine.transform.position.y + 0.5f);

            _porcupine.StartCoroutine(_porcupine.PossibleDrop.SpawnEveryItem(dropPosition, _porcupine.ItemContainer));
        }
    }
}
