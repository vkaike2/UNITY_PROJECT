using System;
using System.Collections;
using UnityEngine;


public class EnemyStatus : MonoBehaviour
{
    [Header("MOBILITY")]
    [SerializeField]
    public EnemyFloatAttribute MovementSpeed;

    [Header("DAMAGE")]
    public EnemyFloatAttribute AtkDamage;
    public EnemyFloatAttribute ImpactDamage;
    [Space]
    public EnemyFloatAttribute MaxHealth;
    [HideInInspector]
    public EnemyFloatAttribute Health;


    public void InitializeHealth()
    {
        Health.Set(MaxHealth.Get());
    }
}


[Serializable]
public class EnemyFloatAttribute
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
