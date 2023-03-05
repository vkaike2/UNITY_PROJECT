using System.Collections;
using UnityEngine;


public class PorcupineProjectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float angle = Mathf.Atan2(_rigidbody2D.velocity.y, _rigidbody2D.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
    }

    public void SetInitialInitialValues(Vector2 velocity, float duration)
    {
        _rigidbody2D.velocity = velocity;
        StartCoroutine(WaitThenDie(duration));
    }

    IEnumerator WaitThenDie (float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
