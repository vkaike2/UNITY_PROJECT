﻿using System;
using UnityEngine;

[Serializable]
public class WormDamageableBehavourModel : EnemyDamageableModel
{
    public bool CanReceiveDamage { get; set; }
}