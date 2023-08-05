﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Fart : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private ParticleSystem _particleSystem;
    [SerializeField]
    private ProgressBarUI _progressBar;
    [Space]
    [SerializeField]
    private Transform _rotationalTransform;

    [Space]
    [Header("CONFIGURATION")]
    [Tooltip("time where the entity will lose the controll, and only this component will aply the knockback")]
    [SerializeField]
    private float _cdwToManipulateKnockBack = 0.3f;
    [SerializeField]
    private float _knockBackForce = 500;
    [SerializeField]
    [Tooltip("increate percentage to horizontal")]
    private float _helpForcePercentage = 1.5f;
    [SerializeField]
    private float _fartCdw = 0.5f;

    [field: Header("EVENTS")]
    [field: Tooltip("Will be called everytime that you fart")]
    [field: SerializeField]
    public OnFartEvent OnFartEvent { get; private set; } = new OnFartEvent();


    private Player _player;
    private Rigidbody2D _rigidbody2D;
    private bool _isFartOnCdw = false;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        InitializeUIManager();
        _player.FartInput.Performed.AddListener(OnFartInputPerformed);
    }

    private void OnFartInputPerformed()
    {
        if (_isFartOnCdw) return;

        (Vector2 position, Vector2 direction, Quaternion rotation) mouse = GetMouseInformationRelatedToGameObject();

        Vector2 fartForce = _knockBackForce * -mouse.direction;

        StartCoroutine(CalculateFartCooldow());
        StartCoroutine(TakeControllOfEntity(mouse.direction));

        fartForce = new Vector2(fartForce.x, fartForce.y * _helpForcePercentage);
        SpawnParticleSystem(mouse);

        // deactivate vertical velocity if you want to go up
        if (fartForce.y > 0)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        }

        _rigidbody2D.AddForce(fartForce);
    }

    //TODO: fart shouldn't be a particle?
    private void SpawnParticleSystem((Vector2 position, Vector2 direction, Quaternion rotation) mouse)
    {
        _particleSystem.transform.rotation = mouse.rotation;
        _particleSystem.Play();
    }

    public (Vector2 position, Vector2 direction, Quaternion rotation) GetMouseInformationRelatedToGameObject()
    {
        Vector2 mousePosition = GetMousePosition();

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        Quaternion mouseRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        return (mousePosition, mousePosition.normalized, mouseRotation);
    }

    private Vector2 GetMousePosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePosition.x -= objectPos.x;
        mousePosition.y -= objectPos.y;
        return mousePosition;
    }

    IEnumerator CalculateFartCooldow()
    {
        _isFartOnCdw = true;
        _progressBar.OnSetBehaviour.Invoke(_fartCdw, ProgressBarUI.Behaviour.ProgressBar_Hide);

        float cdw = 0;

        while (cdw <= _fartCdw)
        {
            cdw += Time.deltaTime;
            UIEventManager.instance.OnPlayerFartProgressBar.Invoke(cdw / _fartCdw);
            yield return new WaitForFixedUpdate();
        }

        _isFartOnCdw = false;
    }

    IEnumerator TakeControllOfEntity(Vector2 mouseDirection)
    {
        _player.CanMove = false;
        bool needToFlipPlayer = CheckIfNeedToFlipEntity(mouseDirection);

        if (needToFlipPlayer)
        {
            _rotationalTransform.localScale = new Vector3(-_rotationalTransform.localScale.x, 1, 1);
        }

        _player.PlayerAnimator.PlayAnimationHightPriority(this, PlayerAnimatorModel.Animation.Fart, _cdwToManipulateKnockBack);
        OnFartEvent.Invoke(_cdwToManipulateKnockBack);

        yield return new WaitForSeconds(_cdwToManipulateKnockBack);

        if (needToFlipPlayer)
        {
            _rotationalTransform.localScale = new Vector3(-_rotationalTransform.localScale.x, 1, 1);
        }

        _player.CanMove = true;
    }

    private bool CheckIfNeedToFlipEntity(Vector2 mouseDirection)
    {
        bool isFacingRight = _rotationalTransform.localScale.x == 1;

        return (!isFacingRight && mouseDirection.x < 0)
            || (isFacingRight && mouseDirection.x > 0);
    }

    private void InitializeUIManager()
    {
        UIEventManager.instance.OnPlayerFartProgressBar.Invoke(1f);
    }
}

/// <summary>
///  This event will be called every time that you fart
///  float: the time that he will stay farting
/// </summary>
[Serializable]
public class OnFartEvent : UnityEvent<float> { }
