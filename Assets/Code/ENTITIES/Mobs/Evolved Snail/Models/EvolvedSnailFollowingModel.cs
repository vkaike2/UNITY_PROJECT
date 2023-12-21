using System;
using UnityEngine;

[Serializable]
public class EvolvedSnailFollowingModel : EnemyFollowModel
{
    [field: Header("ATTACK CHECK")]
    [field: SerializeField]
    public LayerCheckCollider RightAtkCheck { get; set; }
    [field: SerializeField]
    public LayerCheckCollider LeftAtkCheck { get; set; }
}