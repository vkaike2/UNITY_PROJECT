using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class StoneBall : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Transform _rotationalTransform;
    [SerializeField]
    private LayerCheckCollider _layeCheckCollider;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private bool _goingToTheRight = true;
    [SerializeField]
    private int _groundLayerIndex = 3;
    [Space]
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private float _speed = 3f;


    private Rigidbody2D _rigidBody2D;

    private bool _hasTouchedTheGround;

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _layeCheckCollider.OnLayerCheckTriggerEnter.AddListener(OnHitTheWall);
    }

    public void SetDirection(float direction)
    {
        _goingToTheRight = direction == 1;
    }

    private void OnHitTheWall(GameObject wallGameObject)
    {
        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        if (!_hasTouchedTheGround) return;
        _rigidBody2D.velocity = new Vector2(_goingToTheRight ? _speed : -_speed, _rigidBody2D.velocity.y);
        RotateBall();
    }

    private void RotateBall()
    {
        float currentRotation = _rotationalTransform.rotation.eulerAngles.z;

        // Calculate the new rotation angle
        float newRotation = currentRotation + (_goingToTheRight ? _rotationSpeed : -_rotationSpeed) * Time.fixedDeltaTime;

        // Apply the new rotation to the GameObject
        _rotationalTransform.rotation = Quaternion.Euler(0f, 0f, newRotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_hasTouchedTheGround) return;
        if (collision.gameObject.layer != _groundLayerIndex) return;

        _hasTouchedTheGround = true;
    }
}