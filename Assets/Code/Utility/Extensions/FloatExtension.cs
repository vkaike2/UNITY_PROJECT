using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtension
{
    public static bool IsBetween(this float value, float min, float max)
    {
        return value > min && value < max; 
    }

    public static string SecondsToTime(this float seconds)
    {
        // Calculate minutes and seconds
        int minutes = (int)seconds / 60;
        int remainingSeconds = (int)seconds % 60;

        // Format the time as "MM:SS"
        string formattedTime = $"{minutes:D2}:{remainingSeconds:D2}";

        return formattedTime;
    }
}
