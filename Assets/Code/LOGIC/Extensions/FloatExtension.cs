using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtension
{
    public static bool IsBetween(this float value, float min, float max)
    {
        return value > min && value < max; 
    }
}
