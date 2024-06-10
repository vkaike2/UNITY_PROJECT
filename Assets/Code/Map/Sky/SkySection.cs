using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkySection : MonoBehaviour
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

    public bool ImInsideSkyLimit(Vector2 position)
    {
        Border activeBorder = GetActiveBorder();
        return position.x < activeBorder.Right && position.x > activeBorder.Left;
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
        [field: Space]

        [field: SerializeField]
        public float Left { get; private set; }
        [field: SerializeField]
        public float Right { get; private set; }
    }
}