using System;
using UnityEngine.Events;

public class InputModel<T>
{
    public T Value { get; set; }

    public UnityEvent Performed { get; private set; } = new UnityEvent();
    public UnityEvent Canceled { get; private set; } = new UnityEvent();
}
