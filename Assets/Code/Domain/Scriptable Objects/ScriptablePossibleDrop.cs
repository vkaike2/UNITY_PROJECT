using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PossibleDrop", menuName = "ScriptableObjects/Drop")]
public class ScriptablePossibleDrop : ScriptableObject
{
    [field: Header("DROP")]
    [field: SerializeField]
    public PossibleDrop PossibleDrop { get; private set; }

    private void OnValidate()
    {
        if (PossibleDrop == null) return;
        if (PossibleDrop.ItemPools.Count == 0) return;
        foreach (var pool in PossibleDrop.ItemPools)
        {
            pool.Validate();
        }
    }
}
