using System;
using UnityEngine;

[Serializable]
public class ChickenAtkPlayerModel
{
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public  Hitbox AtkHitbox { get; private set; }

    public Action InteractWithPlayer { get; set; }
    public Action EndAtkAnimation { get; set; }
}
