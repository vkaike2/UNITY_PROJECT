using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class EnemyDamageableModel
{
    [Header("component")]
    [SerializeField]
    private ProgressBarUI _progressBarUI;

    [Header("configuration")]
    [SerializeField]
    private float _initialHealth = 5;
    [SerializeField]
    private float _initialDamage = 1;

    public float CurrentHealth { get; set; }

    public ProgressBarUI ProgresBarUI => _progressBarUI;

    public float InitialDamage => _initialDamage;
    public float InitialHealth => _initialHealth;
}
