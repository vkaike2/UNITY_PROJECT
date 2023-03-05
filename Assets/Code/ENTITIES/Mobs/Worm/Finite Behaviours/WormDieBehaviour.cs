using System.Collections;
using UnityEngine;


public class WormDieBehaviour : WormFiniteBaseBehaviour
{
    public override Worm.Behaviour Behaviour => Worm.Behaviour.Die;

    public override void OnEnterBehaviour()
    {
        _worm.Animator.PlayAnimation(WormAnimatorModel.AnimationName.Die);
        _worm.HitBox.gameObject.SetActive(false);
        _rigidbody2D.velocity = Vector3.zero;
        _rigidbody2D.isKinematic = true;
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
}
