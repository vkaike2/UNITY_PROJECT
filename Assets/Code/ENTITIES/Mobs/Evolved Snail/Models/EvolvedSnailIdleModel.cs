using System;
using UnityEngine;

[Serializable]
public class EvolvedSnailIdleModel
{
    [field: Header("UI COMPONENTS")]
    [field: SerializeField]
    public CdwIndicationUI CdwIndicationUI { get; private set; }

    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float RangeToStartFollowingPlayer { get; private set; } = 5;

    [field: Tooltip("Will use this cdw if the player dont come nearby")]
    [field: SerializeField]
    public float CdwToStartFollowingPlayer { get; private set; } = 5;

    public void DrawGizmos(Vector2 myPosition)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(myPosition, RangeToStartFollowingPlayer);
    }
}