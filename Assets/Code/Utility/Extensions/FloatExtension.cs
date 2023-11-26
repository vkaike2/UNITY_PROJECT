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

    public static string SecondsToTime(this float seconds, bool useHour = false)
    {
        float hour = 0;
        float minute = 0;
        float second = 0;

        //has hours
        if (second >= 3600 && useHour)
        {
            hour = seconds / 3600;
            seconds -= 3600 * (int)Math.Floor(hour);
        }

        // has minutes
        if (seconds >= 60)
        {
            minute = seconds / 60;
            seconds -= 60 * (int)Math.Floor(minute);
        }

        second = seconds;

        if (useHour) return $"{hour.ToString("F0").PadLeft(2, '0')}:{minute.ToString("F0").PadLeft(2, '0')}:{second.ToString("F0").PadLeft(2, '0')}";

        return $"{minute.ToString("F0").PadLeft(2, '0')}:{second.ToString("F0").PadLeft(2, '0')}";
    }
}
