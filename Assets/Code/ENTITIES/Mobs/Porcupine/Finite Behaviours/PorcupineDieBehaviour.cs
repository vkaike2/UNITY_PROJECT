using System.Collections;
using UnityEngine;


public class PorcupineDieBehaviour : PorcupineFiniteBaseBehaviour
{
    public override Porcupine.Behaviour Behaviour => Porcupine.Behaviour.Die;

    public override void OnEnterBehaviour()
    {
        _porcupine.AtkModel.CanAtk = false;


        _porcupine.Animator.PlayAnimation(PorcupineAnimatorModel.AnimationName.Die);
        _porcupine.HitBox.gameObject.SetActive(false);
        _rigidbody2D.velocity = Vector3.zero;
        _rigidbody2D.isKinematic = true;
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
}
