using System.Collections;
using UnityEngine;

public class SeagullProjectile : MonoBehaviour
{
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public Hitbox Hitbox { get; private set; }

    [SerializeField]
    private float _duration = 3;

    private Animator _animator;
    private const string FLOOR_ANIMATION = "Seagull_Projectile_Ground";

    private Rigidbody2D _rigidbody2D;

    private const float TIME_TO_START_COUNTING = 0.5f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(ManageFalling());
    }


    private IEnumerator ManageFalling()
    {
        yield return new WaitForSeconds(TIME_TO_START_COUNTING);

        yield return new WaitUntil(() => _rigidbody2D.velocity.y == 0);

        _animator.Play(FLOOR_ANIMATION);

        yield return new WaitForSeconds(_duration);
        Destroy(this.gameObject);
    }
}