using System;
using UnityEngine;

[Serializable]
public class EggIdleBehaviourModel
{
    [Header("CONFIGURATION")] 
    [SerializeField]
    private float _cdwToSpawn;


    public float CdwToSpawn => _cdwToSpawn;   
}