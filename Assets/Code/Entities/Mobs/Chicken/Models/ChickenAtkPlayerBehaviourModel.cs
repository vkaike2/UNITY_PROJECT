using System;
using UnityEngine;

[Serializable]
public class ChickenAtkPlayerBehaviourModel
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Hitbox _atkHitbox;

    public Hitbox AtkHitbox => _atkHitbox;

    public Action InteractWithPlayer { get; set; }
    public Action EndAtkAnimation { get; set; }
}
