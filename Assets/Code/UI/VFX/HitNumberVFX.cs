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
        //float newSacale = _randomScale.GetRandom();
        float newSacale = 5;
        //LoggerUtils.Log(Color.red, newSacale.ToString());
        this.transform.localScale = new Vector3(newSacale, newSacale, newSacale);
    }


    public void SetNumber(float number)
    {
        _label.text = number.ToString();

        foreach (TMP_Text outline in _outlines)
        {
            outline.text = number.ToString();
        }
    }
}
