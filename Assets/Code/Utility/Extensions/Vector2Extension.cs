using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Vector2Extension
{
    public static Vector2 CalculateMiddlePosition(this List<Vector2> positions)
    {
        Vector2 sum = Vector2.zero;
        for (int i = 0; i < positions.Count; i++)
        {
            sum += positions[i];
        }
        Vector2 middlePosition = sum / positions.Count;
        return middlePosition;
    }

    public static int NumericOrder(this Vector2 position)
    {
        string number = $"{position.x}{position.y}";

        return int.Parse(number);
    }
}
