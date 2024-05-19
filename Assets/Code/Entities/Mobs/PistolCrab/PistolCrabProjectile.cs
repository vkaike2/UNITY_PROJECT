using System.Collections;
using UnityEngine;

public class PistolCrabProjectile : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Will be destroyed after this duration, if didn't touch the groud")]
    private float _duration = 5;

    public Hitbox Hitbox { get; private set; }

    private Rigidbody2D _rigidbody2D;
    private LayerCheckCollider _layerCheckCollider;

    private void Awake()
    {
        Hitbox = GetComponent<Hitbox>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _layerCheckCollider = GetComponent<LayerCheckCollider>();
    }

    private void Start()
    {
        _layerCheckCollider.OnLayerCheckTriggerEnter.AddListener(OnCollidingWithGround);
    }

    public void Shoot(Vector2 speed)
    {
        StartCoroutine(DestroyWhenDurationEnd());        
        _rigidbody2D.velocity = speed;
    }

    private void OnCollidingWithGround(GameObject collidingWith)
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    private IEnumerator DestroyWhenDurationEnd()
    {
        yield return new WaitForSeconds(_duration);

        Destroy(this.gameObject);
    }
}