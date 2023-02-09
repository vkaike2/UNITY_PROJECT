
using System;
using UnityEngine;

[Serializable]
public class FartStateModel
{
    [Header("components")]
    [SerializeField]
    private ParticleSystem _particleSystem;
    [SerializeField]
    private ProgressBarUI _progressBar;

    [Space]
    [Header("configuration")]
    [SerializeField]
    private float _cdwToManipulateKnockBack = 0.3f;
    [SerializeField]
    private float _knockBackForce = 500;
    [SerializeField]
    [Tooltip("increate percentage to horizontal")]
    private float _helpForcePercentage = 1.5f;
    [SerializeField]
    private float _fartCdw = 0.5f;

    public ParticleSystem ParticleSystem => _particleSystem;
    public ProgressBarUI ProgressBar => _progressBar;

    public float CdwToManipulateKnockBack => _cdwToManipulateKnockBack;
    public float KnockBackForce => _knockBackForce;
    
    public float HelpForcePercentage => _helpForcePercentage;
    public float FartCdw => _fartCdw;
}
