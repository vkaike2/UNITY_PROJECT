using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Fart : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Transform _fartSpawnTransform;
    [Space]
    [SerializeField]
    private ProgressBarUI _progressBar;
    [Space]
    [SerializeField]
    private Transform _rotationalTransform;

    [Space]
    [Header("CONFIGURATION")]
    [Tooltip("time where the entity will lose the control, and only this component will apply the knockback")]
    [SerializeField]
    private float _cdwToManipulateKnockBack = 0.3f;
    [SerializeField]
    private float _cdwToManipulateKnockBackWithShift = 0.1f;
    [SerializeField]
    private float _knockBackForce = 500;
    [SerializeField]
    [Tooltip("increate percentage to horizontal")]
    private float _helpForcePercentage = 1.5f;

    [field: Header("EVENTS")]
    [field: Tooltip("Will be called every time that you fart")]
    [field: SerializeField]
    public OnKnockbackEvent OnKnockBackEvent { get; private set; } = new OnKnockbackEvent();
    [field: SerializeField]
    public OnFartSpawnedEvent OnFartSpawnedEvent { get; private set; } = new OnFartSpawnedEvent();

    private Player _player;
    private Rigidbody2D _rigidBody2D;
    private bool _isFartOnCdw = false;
    private PlayerStatus.FartStatus _status;
    private GameManager _gameManager;

    private void Awake()
    {
        _status = GetComponent<PlayerStatus>().Fart;
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();

        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        InitializeUIManager();
        _player.FartInput.Performed.AddListener(OnFartInputPerformed);

        _gameManager.OnPlayerDead.AddListener(OnPayerDead);
    }

    private void OnFartInputPerformed()
    {
        if (_isFartOnCdw) return;
        if (_player.CurrentState == Player.FiniteState.Dead) return;
        if (_player.CurrentState == Player.FiniteState.Eating) return;

        (Vector2 position, Vector2 direction, Quaternion rotation) mouse = GetMouseInformationRelatedToGameObject();

        Vector2 fartForce = _knockBackForce * -mouse.direction;

        StartCoroutine(CalculateFartCooldown());
        StartCoroutine(TakeControlOfEntity(mouse.direction));

        fartForce = new Vector2(fartForce.x, fartForce.y * _helpForcePercentage);

        SpawnFartProjectile(mouse);

        if (CheckIfCantApplyKnockback(fartForce)) return;

        // deactivate vertical velocity if you want to go up
        if (fartForce.y > 0)
        {
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0);
        }

        _rigidBody2D.AddForce(fartForce);
    }

    private bool CheckIfCantApplyKnockback(Vector2 fartForce)
    {
        Vector2 normalizedForce = fartForce.normalized;
        bool knockBackUp = normalizedForce.y > 0.85;

        return _player.IsTouchingGround && !knockBackUp;
    }

    private void SpawnFartProjectile((Vector2 position, Vector2 direction, Quaternion rotation) mouse)
    {
        FartProjectile projectile = Instantiate(_status.Projectile.Get(), _fartSpawnTransform);
        projectile.transform.parent = null;
        projectile.transform.rotation = mouse.rotation;
        projectile.SetInitialValues(mouse.direction * _status.Velocity.Get(), _player);
    }

    private (Vector2 position, Vector2 direction, Quaternion rotation) GetMouseInformationRelatedToGameObject()
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

    IEnumerator CalculateFartCooldown()
    {
        _isFartOnCdw = true;
        _progressBar.OnSetBehaviour.Invoke(_status.Cooldown.Get(), ProgressBarUI.Behaviour.ProgressBar_Hide);

        float cdw = 0;

        while (cdw <= _status.Cooldown.Get())
        {
            cdw += Time.deltaTime;
            UIEventManager.instance.OnPlayerFartProgressBar.Invoke(cdw / _status.Cooldown.Get());
            yield return new WaitForFixedUpdate();
        }

        _isFartOnCdw = false;
    }

    IEnumerator TakeControlOfEntity(Vector2 mouseDirection)
    {
        bool needToFlipPlayer = CheckIfNeedToFlipEntity(mouseDirection);

        if (needToFlipPlayer)
        {
            _rotationalTransform.localScale = new Vector3(-_rotationalTransform.localScale.x, 1, 1);
        }

        _player.PlayerAnimator.PlayAnimationHightPriority(this, PlayerAnimatorModel.Animation.Fart, _cdwToManipulateKnockBack);

        OnKnockBackEvent.Invoke(_cdwToManipulateKnockBack, KnockBackSource.Fart);

        yield return new WaitForSeconds(_cdwToManipulateKnockBack);

        if (needToFlipPlayer)
        {
            _rotationalTransform.localScale = new Vector3(-_rotationalTransform.localScale.x, 1, 1);
        }
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

    private void OnPayerDead(string damageSource)
    {
        StopAllCoroutines();
        _isFartOnCdw = true;
    }
}

[SerializeField]
public class OnFartSpawnedEvent : UnityEvent<FartProjectile> { }
