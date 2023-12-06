using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitNumberVFX : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private TMP_Text _label;
    [Space]
    [SerializeField]
    private List<TMP_Text> _outlines;
    [Space]
    [SerializeField]
    private MinMax _randomScale;

    private void Awake()
    {
        float newScale = _randomScale.GetRandom();
        this.transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public void SetNumber(float number)
    {
        string textNumber = number.ToString();

        //if(number < 0)
        //{
        //    textNumber = number.ToString("0.00");
        //}

        _label.text = textNumber;

        foreach (TMP_Text outline in _outlines)
        {
            outline.text = textNumber;
        }
    }
}
