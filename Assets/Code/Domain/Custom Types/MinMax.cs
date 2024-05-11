using System;
using UnityEngine;

[Serializable]
public struct MinMax
{
    [SerializeField]
    private float _min;
    [SerializeField]
    private float _max;

    public MinMax(float min, float max)
    {
        _min = min;
        _max = max;
    }

    public readonly float Min => _min;
    public readonly float Max => _max;
    public readonly float Range => _max - _min;

    public readonly float GetRandom() => UnityEngine.Random.Range(_min, _max);
    public readonly int GetRandomInt() => UnityEngine.Random.Range((int)_min, ((int)_max) +1);
}
