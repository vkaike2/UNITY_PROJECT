using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class RaycastModel
{
    [SerializeField]
    private float _colliderRadius = 0.9f;
    [SerializeField]
    private Transform _transform;


    public float ColliderRadius => _colliderRadius;
    public Transform GroundRaycastTransform => _transform;

    public void DrawGizmos(Color color, bool horizontal = true)
    {
        Gizmos.color = color;
        if (horizontal)
        {
            Gizmos.DrawWireCube(_transform.position, new Vector3(_colliderRadius, _colliderRadius / 2, 0));
        }
        else
        {
            Gizmos.DrawWireCube(_transform.position, new Vector3(_colliderRadius / 2, _colliderRadius, 0));
        }

    }

    public Collider2D DrawPhysics2D(LayerMask colisionLayer, bool horizontal = true)
    {
        if (horizontal)
        {
            return Physics2D.OverlapArea(
                new Vector2(_transform.position.x - _colliderRadius / 4, _transform.position.y - _colliderRadius / 2),
                new Vector2(_transform.position.x + _colliderRadius / 4, _transform.position.y + _colliderRadius / 2),
                colisionLayer);
        }
        else
        {
            return Physics2D.OverlapArea(
                new Vector2(_transform.position.x - _colliderRadius / 2, _transform.position.y - _colliderRadius / 4),
                new Vector2(_transform.position.x + _colliderRadius / 2, _transform.position.y + _colliderRadius / 4),
                colisionLayer);
        }

    }
}
