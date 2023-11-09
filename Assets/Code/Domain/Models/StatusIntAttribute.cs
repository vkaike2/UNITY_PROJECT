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

    public void Add(int value)
    {
        SetCurrentValue();

        _currentValue += value;
    }

    public void Remove(int value)
    {
        SetCurrentValue();

        _currentValue -= value;
    }

    private void SetCurrentValue() => _currentValue ??= _value;
}
