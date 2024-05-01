using System.Collections;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    [Header("CONFIGURATION")]
    [SerializeField]
    private float _duration = 2;
    [SerializeField]
    private float _timeToHide = 1;

    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public Hitbox Hitbox { get; private set; }

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private readonly string ANIMATION_HIDE = "Shockwave_Down";


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(WaitThenHide());
    }

    private IEnumerator WaitThenHide()
    {
        yield return new WaitForSeconds(_duration);

        _animator.Play(ANIMATION_HIDE);

        float cdw = 0;
        while (cdw <= _timeToHide)
        {
            cdw += Time.deltaTime;
            var currentColor = _spriteRenderer.color;
            currentColor.a = Mathf.Lerp(1, 0, cdw / _timeToHide);
            _spriteRenderer.color = currentColor;

            yield return new WaitForFixedUpdate();
            Destroy(this.gameObject, 0.2f);
        }
    }
}