using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class EnemyDamageableModel
{
    [Header("component")]
    [SerializeField]
    private ProgressBarUI _progressBarUI;

    public ProgressBarUI ProgresBarUI => _progressBarUI;
}
