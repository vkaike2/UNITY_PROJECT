using UnityEngine;

public class GunGarooBullet : MonoBehaviour
{
    private Rigidbody2D _rigidBody2D;
    private LayerCheckCollider _layerCheckCollider;

    private void Awake()
    {
        _layerCheckCollider = GetComponent<LayerCheckCollider>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _layerCheckCollider.OnLayerCheckTriggerEnter.AddListener(DestroyProjectileOnCollision);
    }

    public void SetInitialInitialValues(Vector2 velocity)
    {
        this.transform.localScale = new Vector3(velocity.x > 1 ? 1 : -1, 1, 1);

        _rigidBody2D.velocity = velocity;
    }

    private void DestroyProjectileOnCollision(GameObject arg0)
    {
        Destroy(gameObject);
    }
}
