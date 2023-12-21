using UnityEngine;

public class EvolvedSnailProjectile : MonoBehaviour
{
    private Rigidbody2D _rigidBody2D;
    private LayerCheckCollider _layerCheckCollider;

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _layerCheckCollider = GetComponent<LayerCheckCollider>();
    }

    private void Start()
    {
        _layerCheckCollider.OnLayerCheckTriggerEnter.AddListener(DestroyProjectileOnCollision);
    }

    private void FixedUpdate()
    {
        float angle = Mathf.Atan2(_rigidBody2D.velocity.y, _rigidBody2D.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.localScale = new Vector3(1, -1, 1);
    }

    public void SetInitialInitialValues(Vector2 velocity)
    {
        _rigidBody2D.velocity = velocity;
    }

    private void DestroyProjectileOnCollision(GameObject gameObject)
    {
        Destroy(base.gameObject);
    }
}