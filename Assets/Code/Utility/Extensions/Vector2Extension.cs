using System.Collections;
using UnityEngine;


public static class Vector2Extension
{
    public static Vector2 Normalized(this Vector2 vector)
    {
        Vector2 normalizedVector = Vector2.zero;

        if (vector.x > 0)
        {
            normalizedVector.x = 1;
        }
        else if (vector.x < 0)
        {
            normalizedVector.x = -1;
        }
        else if(vector.x == 0)
        {
            normalizedVector.x = 0;
        }

        if (vector.y > 0)
        {
            normalizedVector.y = 1;
        }
        else if (vector.y < 0)
        {
            normalizedVector.y = -1;
        }
        else if (vector.y == 0)
        {
            normalizedVector.y = 0;
        }

        return normalizedVector;
    }
}
