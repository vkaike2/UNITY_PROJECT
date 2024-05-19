using UnityEngine;

public class WaterSection : MonoBehaviour
{
    [field: Header("CORNERS")]
    [field: SerializeField]
    public Transform TopRight { get; set; }
    [field: SerializeField]
    public Transform TopLeft { get; set; }
    [field: SerializeField]
    public Transform BottomRight { get; set; }
    [field: SerializeField]
    public Transform BottomLeft { get; set; }


    public bool ImUnderWater(Vector2 position)
    {
        bool horizontalCheck = position.x > TopLeft.position.x && position.x < BottomRight.position.x;
        bool verticalCheck = position.y > BottomLeft.position.y && position.y < TopRight.position.y;

        return verticalCheck && horizontalCheck;
    }

    public Vector2 GiveMeNextDirection(Vector2 position)
    {
        var leftDistance = Vector2.Distance(position, TopLeft.position);
        var rightDistance = Vector2.Distance(position, TopRight.position);

        if (leftDistance > rightDistance)
        {
            return new Vector2(-1, 0);
        }

        return new Vector2(1, 0);
    }

    public float GetRandomVerticalPosition()
    {
        float maxHeight = TopLeft.position.y;
        float minHeight = BottomLeft.position.y;

        return UnityEngine.Random.Range(minHeight, maxHeight);
    }
}