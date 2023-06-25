using Assets.Code.LOGIC;
using System;
using UnityEngine;

[Serializable]
public class PorcupinePatrolBehaviourModel : EnemyPatrolModel
{
    [Header("COMPONENTS")]
    [SerializeField]
    private LayerCheckCollider _layerCheckCollider;
    
    public LayerCheckCollider LayerCheckCollider => _layerCheckCollider;
}
