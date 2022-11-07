
using System;
using UnityEngine;

[Serializable]
public class FartAttributeModel
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
    [Tooltip("Reduce percentage from horizontal force")]
    private float _helpForcePercentage = 0.8f;
    [SerializeField]
    private float _fartCdw = 0.5f;

    public ParticleSystem ParticleSystem => _particleSystem;
    public ProgressBarUI ProgressBar => _progressBar;

    public float CdwToManipulateKnockBack => _cdwToManipulateKnockBack;
    public float KnockBackForce => _knockBackForce;
    
    public float HelpForcePercentage => _helpForcePercentage;
    public float FartCdw => _fartCdw;
}
