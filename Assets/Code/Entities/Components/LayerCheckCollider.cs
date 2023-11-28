using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class LayerCheckCollider : MonoBehaviour
{
    [Header("configuration")]
    [SerializeField]
    private CollisionType _collisionType;

    [Header("collider configuration")]
    [Space]
    [SerializeField]
    private List<CollisionLayer> _collisonLayers;

    [Header("raycast configuration")]
    [Space]
    [SerializeField]
    private LayerMask _layerMask;
    [Space]
    [SerializeField]
    private float _horizontalRaycastSize = 0.5f;

    public bool IsRaycastCollidingWithLayer => CheckRaycastColision();
    public OnLayerCheckTriggerEnter OnLayerCheckTriggerEnter { get; private set; }

    private void OnValidate()
    {
        if (_collisonLayers != null && _collisonLayers.Count() == 0) return;

        foreach (var layer in _collisonLayers)
        {
            layer.name = LayerMask.LayerToName(layer.LayerIndex);
        }
    }

    private void Awake()
    {
        OnLayerCheckTriggerEnter = new OnLayerCheckTriggerEnter();
    }

    private void OnDrawGizmos()
    {
        if (_collisionType == CollisionType.Collider) return;

        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - _horizontalRaycastSize));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_collisionType != CollisionType.Collider) return;

        GameObject gameObject = FindLayerInParent(collision.gameObject, _collisonLayers.Select(e => e.LayerIndex).ToList());
        if (gameObject == null) return;

        OnLayerCheckTriggerEnter.Invoke(gameObject);
    }


    private GameObject FindLayerInParent(GameObject gameObject, List<int> layerIndexs)
    {
        if (layerIndexs.Contains(gameObject.layer))
        {
            return gameObject;
        }

        Transform parent = gameObject.transform.parent;

        if (parent == null) return null;

        return FindLayerInParent(parent.gameObject, layerIndexs);
    }

    private bool CheckRaycastColision()
    {
        RaycastHit2D col = Physics2D.Linecast(transform.position, new Vector2(transform.position.x, transform.position.y - _horizontalRaycastSize), _layerMask);
        return col.collider != null;
    }

    private enum CollisionType
    {
        Raycast,
        Collider
    }

    [Serializable]
    private class CollisionLayer
    {
        [HideInInspector]
        public string name;

        [SerializeField]
        private int _layerIndex;

        public int LayerIndex => _layerIndex;
    }
}


/// <summary>
/// GameObject - colliding with this
/// </summary>
public class OnLayerCheckTriggerEnter : UnityEvent<GameObject> { }