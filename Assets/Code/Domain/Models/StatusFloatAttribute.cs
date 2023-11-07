
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
        SetCurrentValue();

        return _currentValue.Value;
    }

    public void Set(float value)
    {
        _currentValue = value;
    }

    public void IncreaseFlatValue(float value)
    {
        SetCurrentValue();

        _currentValue += value;
    }

    public void IncreasePercentage(float percentage)
    {
        SetCurrentValue();

        if(_currentValue == 0)
        {
            _currentValue += percentage;
            return;
        }

        percentage += 1;
        
        _currentValue *= percentage;
    }

    public void ReduceFlatValue(float value)
    {
        SetCurrentValue();

        _currentValue -= value;
    }
    
    public void ReducePercentage(float percentage)
    {
        SetCurrentValue();

        float percentageValue = _currentValue.Value * percentage;

        _currentValue -= percentageValue;
    }

    public void ResetToDefault()
    {
        _currentValue = _value;
    }

    private void SetCurrentValue() => _currentValue ??= _value;

}
