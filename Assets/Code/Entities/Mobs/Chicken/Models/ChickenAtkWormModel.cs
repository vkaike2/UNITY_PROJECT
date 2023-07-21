using System;
using UnityEngine;

[Serializable]
public class ChickenAtkWormModel
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float EggVelocity { get; private set; } = 3f;

    [field: SerializeField]
    public Egg ChickensEgg { get; private set; }
    [field: SerializeField]
    public Transform SpawnerPosition { get; private set; } 

    public Worm WormTarget { get; set; }

    /// <summary>
    ///     Used to connect the annimator events to the behaviours
    /// </summary>
    public Action InteractWithWorm { get; set; }
    public Action EndAtkAnimation { get; set; }
    public Action ThrowingEgg { get; set; }
}
