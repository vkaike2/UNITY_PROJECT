using System;
using UnityEngine;

[Serializable]
public class PorcupinePatrolModel : EnemyPatrolModel
{
    [Header("COMPONENTS")]
    [SerializeField]
    private LayerCheckCollider _layerCheckCollider;
    
    public LayerCheckCollider LayerCheckCollider => _layerCheckCollider;
}
