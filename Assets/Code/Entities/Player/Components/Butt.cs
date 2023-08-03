using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// TODO: Butt should take controll of the player while pooping
public class Butt : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private PoopProjectile _poopProjectilePrefab;

    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    private ProgressBarUI _cdwProgressBar;

    [Space]
    [Header("PROJECTILE TRAJECTORY")]
    [SerializeField]
    private GameObject _poopTrajectoryPrefab;
    [SerializeField]
    private int _numberOfDots = 50;

    [Space]
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _maximumVelocity = 10;
    [SerializeField]
    [Tooltip("How many secconds it takes to go from 0 to maximum velocity")]
    private float _velocityTimer = 1;
    [SerializeField]
    private float _cdwToPoop = 3f;
    [SerializeField]
    private float _gravityWhilePooping = 0.1f;

    [Space]
    [Header("RAYCAST")]
    [SerializeField]
    private LayerMask _groundLayer;

    [field: Space]
    [field: Header("EVENTS")]
    [field: SerializeField]
    public OnPoopEvent OnPoopEvent { get; private set; } = new OnPoopEvent();

    private Transform _trajectoryParent;
    private UIEventManager _uiEventManager;

    private Rigidbody2D _rigidbody2D;
    private Player _player;

    private GameObject[] _fullTrajectory;

    private bool _canPoop = true;
    private bool _isPooping = false;
    private float _initialGravity;
    private Vector2 _previousVelocity;
    private bool _hasBeingFlipped = false;
    private float _velocityMultiplyer;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _trajectoryParent = FindObjectOfType<PoopProjectileParent>().transform;
        _uiEventManager = FindObjectOfType<UIEventManager>();

        InstantiateFullTrajectory();

        _player.PoopInput.Performed.AddListener(OnPoopStarted);
        _player.PoopInput.Canceled.AddListener(OnPoopStoped);
    }

    private void FixedUpdate()
    {
        ControlPlayerWhilePooping();
    }

    private void ControlPlayerWhilePooping()
    {
        if (!_player.PoopInput.Value) return;

        _rigidbody2D.gravityScale = _gravityWhilePooping;

        if (_previousVelocity.y > 0)
        {
            _rigidbody2D.velocity = Vector2.zero;
        }

        for (int i = 0; i < _numberOfDots; i++)
        {
            _fullTrajectory[i].SetActive(false);
        }

        for (int i = 0; i < _numberOfDots; i++)
        {
            _fullTrajectory[i].transform.position = CalculateDotTrajectory(i * 0.1f);
            RaycastHit2D[] hit = Physics2D.RaycastAll(_fullTrajectory[i].transform.position, -Vector2.up, 0.1f, _groundLayer);

            if (hit.Any())
            {
                break;
            }

            _fullTrajectory[i].SetActive(true);
        }
    }

    private Vector2 CalculateDotTrajectory(float time)
    {
        return (Vector2)_spawnPoint.position + (CalculateVelocityDirection() * time) + (time * time) * 0.5f * Physics2D.gravity;
    }

    private void InstantiateFullTrajectory()
    {
        _fullTrajectory = new GameObject[_numberOfDots];

        for (int i = 0; i < _numberOfDots; i++)
        {
            _fullTrajectory[i] = GameObject.Instantiate(_poopTrajectoryPrefab, _spawnPoint.position, Quaternion.identity);
            _fullTrajectory[i].transform.parent = _trajectoryParent;
            _fullTrajectory[i].SetActive(false);
        }
    }

    private void OnPoopStoped()
    {
        if (!_isPooping) return;

        ThrowPoop();

        _player.CanMove = true;

        if (_hasBeingFlipped)
        {
            FlipPlayer();
            _hasBeingFlipped = false;
        }

        _rigidbody2D.gravityScale = _initialGravity;
        _rigidbody2D.velocity = _previousVelocity;

        _isPooping = false;
    }

    private void OnPoopStarted()
    {
        if (!_canPoop) return;

        _isPooping = true;
        PreparePlayerToPoop();

        _player.PlayerAnimator.PlayAnimation(PlayerAnimatorModel.Animation.Pooping);

        StartCoroutine(CalculatePoopVelocity());
    }

    #region START POOPING PROCESS
    private void PreparePlayerToPoop()
    {
        _initialGravity = _rigidbody2D.gravityScale;
        _previousVelocity = _rigidbody2D.velocity;

        StopPlayer();

        Vector2 mouseDirection = GetMouseDirectionRelatedToPlayer();

        if (CheckIfNeedToFlipPlayer(mouseDirection))
        {
            FlipPlayer();
            _hasBeingFlipped = true;
        }
    }
    private IEnumerator CalculatePoopVelocity()
    {
        float timer = 0;
        _velocityMultiplyer = 0;

        while (timer <= _velocityTimer)
        {
            timer += Time.deltaTime;
            _velocityMultiplyer = timer / _velocityTimer;
            yield return new WaitForFixedUpdate();
        }
        _velocityMultiplyer = 1;
    }
    private void StopPlayer()
    {
        _player.CanMove = false;
        _rigidbody2D.velocity = Vector2.zero;
    }
    #endregion

    #region THROW POOP
    private void ThrowPoop()
    {
        PlayPoopSoundEffect();

        for (int i = 0; i < _numberOfDots; i++)
        {
            _fullTrajectory[i].SetActive(false);
        }

        Vector2 velocityDirection = CalculateVelocityDirection();

        PoopProjectile projectile = Instantiate(_poopProjectilePrefab, _spawnPoint.position, Quaternion.identity);

        OnPoopEvent?.Invoke(projectile);

        projectile.SetVelocity(velocityDirection);

        _player.StartCoroutine(CalculateCooldown());
    }
    #endregion

    private Vector2 GetMouseDirectionRelatedToPlayer()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(_player.transform.position);
        mousePosition.x -= objectPos.x;
        mousePosition.y -= objectPos.y;
        return mousePosition.normalized;
    }

    private bool CheckIfNeedToFlipPlayer(Vector2 mouseDirection)
    {
        bool isFacingRight = _player.RotationalTransform.localScale.x == 1;
        return (!isFacingRight && mouseDirection.x < 0) || (isFacingRight && mouseDirection.x > 0);
    }

    private void FlipPlayer()
    {
        _player.RotationalTransform.localScale = new Vector3(-_player.RotationalTransform.localScale.x, 1, 1);
    }

    private Vector2 CalculateVelocityDirection()
    {
        Vector2 mouseDirection = GetMouseDirectionRelatedToPlayer();
        float currentVelocity = _maximumVelocity * _velocityMultiplyer;
        return currentVelocity * mouseDirection;
    }

    private void PlayPoopSoundEffect()
    {
        if (_player.AudioController == null) return;
        _player.AudioController.PlayClip(AudioController.ClipName.Player_Poop);
    }

    private IEnumerator CalculateCooldown()
    {
        _canPoop = false;
        _cdwProgressBar.OnSetBehaviour.Invoke(_cdwToPoop, ProgressBarUI.Behaviour.ProgressBar_Hide);

        float cdw = 0;
        while (cdw <= _cdwToPoop)
        {
            cdw += Time.deltaTime;

            _uiEventManager.OnPlayerPoopProgressBar.Invoke(cdw / _cdwToPoop);
            yield return new WaitForFixedUpdate();
        }

        _canPoop = true;
    }
}

[Serializable]
public class OnPoopEvent : UnityEvent<PoopProjectile> { }
