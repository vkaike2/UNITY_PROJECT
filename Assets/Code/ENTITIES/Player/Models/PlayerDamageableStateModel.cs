using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class PlayerDamageableStateModel 
{
    [Header("components")]
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private ParticleSystem _fartParticleSystem;

    [Space]
    [Header("configuration")]
    [SerializeField]
    private int _initalHealth = 10;
    [SerializeField]
    private float _fartDamage = 0.2f;
    [SerializeField]
    private float _poopDamage = 2.5f;
    [SerializeField]
    private float _knockBackForce = 700;
    [SerializeField]
    [Tooltip("you need to wait for this amout of time to be able to attack again")]
    private float _cdwToAtkAfterDamage = 1f;

    public float CurrentHealth { get; set; }
    public bool CanAtk { get; set; }

    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public ParticleSystem FartParticleSystem => _fartParticleSystem;

    public float InitialHealth => _initalHealth;
    public float FartDamage => _fartDamage;
    public float PoopDamage => _poopDamage;
    public float KnockBackForce => _knockBackForce;
    public float CdwToAtkAfterDamage => _cdwToAtkAfterDamage;
}
