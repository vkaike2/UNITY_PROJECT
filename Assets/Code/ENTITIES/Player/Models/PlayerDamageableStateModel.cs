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

    public float CurrentHealth { get; set; }

    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public ParticleSystem FartParticleSystem => _fartParticleSystem;

    public float InitialHealth => _initalHealth;
    public float FartDamage => _fartDamage;
    public float PoopDamage => _poopDamage;
}
