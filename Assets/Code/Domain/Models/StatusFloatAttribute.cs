
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

    public void Add(float value)
    {
        SetCurrentValue();

        _currentValue += value;
    }

    public void Remove(float value)
    {
        SetCurrentValue();

        _currentValue -= value;
    }

    public void ResetToDefault()
    {
        _currentValue = _value;
    }

    private void SetCurrentValue() => _currentValue ??= _value;

}
