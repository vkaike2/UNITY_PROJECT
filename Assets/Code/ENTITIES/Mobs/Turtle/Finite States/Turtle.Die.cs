
using System.Collections;
using UnityEngine;

public partial class Turtle : Enemy
{
    private class Die : TurtleBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Die;


        public override void OnEnterBehaviour()
        {
            _turtle.Animator.PlayAnimation(TurtleAnimatorModel.AnimationName.Turtle_Die);
            _turtle.AttackModel.TurtleGun.gameObject.SetActive(false);
            
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
        
            DropItemIfPossible();

            _turtle.StartCoroutine(FadeOutThenDie());
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
            Color color = _turtle.SpriteRenderer.color;
            while (cdw >= 0)
            {
                cdw -= Time.deltaTime;

                color.a = cdw / fadeOutTime;
                _turtle.SpriteRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            GameObject.Destroy(_turtle.gameObject);
        }

        private void DropItemIfPossible()
        {
            if (_turtle.PossibleDrop == null) return;
            if (_turtle.PossibleDrop.ItemPools.Count == 0) return;

            Vector2 dropPosition = new Vector2(_turtle.transform.position.x, _turtle.transform.position.y + 0.5f);

            _turtle.StartCoroutine(_turtle.PossibleDrop.SpawnEveryItem(dropPosition, _turtle.ItemContainer));
        }
    }
}