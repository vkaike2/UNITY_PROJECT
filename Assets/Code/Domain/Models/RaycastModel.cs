using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class RaycastModel
{
    [Header("GIZMO")]
    [SerializeField]
    private bool _showGizmo = true;
    [SerializeField]
    private Color _gizmoColor = Color.red;
    

    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float Height { get; private set; } = 0.4f;
    [field: SerializeField]
    public float Widith { get; private set; } = 0.9f;

    [field: SerializeField]
    public Transform GroundRaycastTransform { get; private set; }


    public void DrawGizmos()
    {
        if (!_showGizmo) return;

        _gizmoColor.a = 1;
        Gizmos.color = _gizmoColor;

        Gizmos.DrawWireCube(GroundRaycastTransform.position, new Vector3(Widith, Height, 0));
    }

    public Collider2D DrawPhysics2D(LayerMask colisionLayer)
    {
        return Physics2D.OverlapArea(
                  new Vector2(GroundRaycastTransform.position.x - Widith / 2, GroundRaycastTransform.position.y - Height / 2),
                  new Vector2(GroundRaycastTransform.position.x + Widith / 2, GroundRaycastTransform.position.y + Height / 2),
                  colisionLayer);
    }
}
