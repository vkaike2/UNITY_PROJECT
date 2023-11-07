using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class StatusIntAttribute
{
    [SerializeField]
    private int _value;

    private int? _currentValue = null;

    public int Get()
    {
        SetCurrentValue();

        return _currentValue.Value;
    }

    public void Set(int value)
    {
        _currentValue = value;
    }

    public void IncreaseFlatValue(int value)
    {
        SetCurrentValue();

        _currentValue += value;
    }

    public void IncreasePercentage(float percentage)
    {
        SetCurrentValue();

        percentage += 1;

        var percentageValue = _currentValue * percentage;

        _currentValue += (int)percentageValue;
    }

    public void ReduceFlatValue(int value)
    {
        SetCurrentValue();

        _currentValue -= value;
    }

    public void ReducePercentage(int percentage)
    {
        SetCurrentValue();

        float percentageValue = _currentValue.Value * percentage;

        _currentValue -= (int)percentageValue;
    }

    private void SetCurrentValue() => _currentValue ??= _value;
}
