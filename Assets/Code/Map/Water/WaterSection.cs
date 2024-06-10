using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterSection : MonoBehaviour
{
    [field: Header("BORDERS")]
    [field: SerializeField]
    public List<Border> Borders { get; set; }

    private void OnValidate()
    {
        if (Borders == null) return;

        int count = 0;
        foreach (var border in Borders)
        {
            border.Index = count;
            count++;
        }

    }

    public void ActivateBorder(int index)
    {
        foreach (var border in Borders)
        {
            border.Active = false;
        }

        Border activeBorder = Borders.FirstOrDefault(e => e.Index == index);
        activeBorder.Active = true;
    }

    public bool ImUnderWater(Vector2 position)
    {
        Border activeBorder = GetActiveBorder();

        bool horizontalCheck = position.x > activeBorder.TopLeft.x && position.x < activeBorder.BottomRight.x;
        bool verticalCheck = position.y > activeBorder.BottomLeft.y && position.y < activeBorder.TopRight.y;

        return verticalCheck && horizontalCheck;
    }

    public float GetRandomVerticalPosition()
    {
        Border activeBorder = GetActiveBorder();

        float maxHeight = activeBorder.TopLeft.y;
        float minHeight = activeBorder.BottomLeft.y;

        return UnityEngine.Random.Range(minHeight, maxHeight);
    }

    private Border GetActiveBorder() => Borders.FirstOrDefault(e => e.Active = true);

    [Serializable]
    public class Border
    {
        [SerializeField]
        private string _description;
        [field: Space(2)]
        
        [field: SerializeField]
        public int Index { get; set; }
        [field: SerializeField]
        public bool Active { get; set; }
        [Space]

        [SerializeField]
        private float _up;
        [SerializeField]
        private float _down;
        [SerializeField]
        private float _left;
        [SerializeField]
        private float _right;
        

        public Vector2 TopRight => new Vector2(_right, _up);
        public Vector2 TopLeft => new Vector2(_left, _up);
        public Vector2 BottomRight => new Vector2(_right, _down);
        public Vector2 BottomLeft => new Vector2(_left, _down);
    }

}