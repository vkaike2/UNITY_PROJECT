using System;
using UnityEngine;


public class EnemyStatus : ImpactDamageStatus
{
    [Header("MOBILITY")]
    [SerializeField]
    public StatusFloatAttribute MovementSpeed;

    [Header("GENERIC DAMAGE")]
    public StatusFloatAttribute AtkDamage;
   
}