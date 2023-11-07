using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public static class LoggerUtils
{
    public static void Log(string message)
    {
        Debug.Log(message);
    }

    public static void Log(Color color, string message)
    {
        Debug.Log($"<color=#{color.ToHexString()}>{message}</color>");
    }
}
