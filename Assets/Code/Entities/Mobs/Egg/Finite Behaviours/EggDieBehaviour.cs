using System.Collections;
using UnityEngine;


public class EggDieBehaviour : EggFiniteBaseBehaviour
{
    public override Egg.Behaviour Behaviour => Egg.Behaviour.Die;

    public override void OnEnterBehaviour()
    {
        _egg.Animator.PlayAnimation(EggAnimatorModel.AnimationName.Die);
        _egg.HitBox.gameObject.SetActive(false);
        _rigidbody2D.velocity = Vector3.zero;
        _rigidbody2D.isKinematic = true;

        _egg.StartCoroutine(FadeOutThenDie());
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
        Color color = _egg.SpriteRenderer.color;
        while (cdw >= 0)
        {
            cdw -= Time.deltaTime;

            color.a = cdw / fadeOutTime;
            _egg.SpriteRenderer.color = color;
            yield return new WaitForFixedUpdate();
        }

        GameObject.Destroy(_egg.gameObject);
    }
}
