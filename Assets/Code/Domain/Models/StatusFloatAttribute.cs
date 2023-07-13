
using System;
using UnityEngine;

[Serializable]
public class StatusFloatAttribute
{
    [SerializeField]
    private float _value;

    private float? _currentValue = null;

    public float Get()
    {
        _currentValue ??= _value;

        return _currentValue.Value;
    }

    public void Set(float value)
    {
        _currentValue = value;
    }

    public void IncreaseFlatValue(float value)
    {
        _currentValue ??= _value;
        _currentValue += value;
    }

    public void IncreasePercentage(float percentage)
    {
        _currentValue ??= _value;
        percentage += 1;
        _currentValue *= percentage;
    }

    public void ReduceFlatValue(float value)
    {
        _currentValue -= value;
    }

    public void ReducePercentage(float percentage)
    {
        float percentageValue = _currentValue.Value * percentage;

        _currentValue -= percentageValue;
    }
}
