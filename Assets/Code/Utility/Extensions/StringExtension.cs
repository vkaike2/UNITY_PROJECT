using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public static class StringExtension
{
    public static string Paint(this string message, Color color)
    {
        return $"<color=#{color.ToHexString()}>{message}</color>";
    }
}
