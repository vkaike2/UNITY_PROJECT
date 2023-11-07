using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class StatusMonobehaviourAttribute<T> where T : MonoBehaviour
{
    [SerializeField]
    private T _initial;

    private T _value;

    public T Get()
    {
        if( _value == null )
        {
            _value = _initial;
        }    

        return _value;
    }

    public void Set(T monobehaviour)
    {
        _value = monobehaviour;
    }

    public void Reset()
    {
        _value = _initial;
    }
}
