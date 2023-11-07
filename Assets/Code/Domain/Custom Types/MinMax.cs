using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MinMax
{
    [SerializeField]
    private float _min;
    [SerializeField]
    private float _max;

    public readonly float Min => _min;
    public readonly float Max => _max;
    public readonly float Range => _max - _min;

    public readonly float GetRandom() => UnityEngine.Random.Range(_min, _max);
    public readonly int GetRandomInt() => UnityEngine.Random.Range((int)_min, (int)_max);
}
