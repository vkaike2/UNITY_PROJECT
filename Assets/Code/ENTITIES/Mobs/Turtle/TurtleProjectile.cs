using System.Collections;
using UnityEngine;

public class TurtleProjectile : MonoBehaviour
{

    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public Hitbox Hitbox { get; private set; }

    private Rigidbody2D _rigidBody2D;

    private Vector2 _lastVelocity;
    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
    }

    public void SetInitialInitialValues(Vector2 velocity, float duration)
    {
        _rigidBody2D.velocity = velocity;
        _lastVelocity = velocity;
        StartCoroutine(WaitForDurationToDie(duration));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float speed = _lastVelocity.magnitude;
        Vector2 direction = Vector2.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);

        _rigidBody2D.velocity = direction * speed;
        _lastVelocity = _rigidBody2D.velocity;
    }


    private IEnumerator WaitForDurationToDie(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }

}