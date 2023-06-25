using System;
using UnityEngine;

[Serializable]
public class ChickenAtkWormBehaviourModel
{
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _eggVelocity = 3f;

    [SerializeField]
    private Egg _chickensEgg;
    [SerializeField]
    private Transform _spawnerPosition; 

    public float EggVelocity => _eggVelocity;

    public Egg ChickensEgg => _chickensEgg;
    public Transform SpawnerPosition => _spawnerPosition;

    public Worm WormTarget { get; set; }
    public Action InteractWithWorm { get; set; }
    public Action EndAtkAnimation { get; set; }
    public Action ThrowingEgg { get; set; }
}
