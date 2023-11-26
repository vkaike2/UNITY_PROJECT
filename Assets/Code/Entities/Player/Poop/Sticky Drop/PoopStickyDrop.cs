using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;


public class PoopStickyDrop : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private List<Transform> _auxiliarPositions;
    [SerializeField]
    private Hitbox _hitbox;
    [SerializeField]
    private Transform _hitboxTransform;
    [SerializeField]
    private BoxCollider2D _hitboxCollider;

    [Space]
    [SerializeField]
    PoopStickyDropParticle _stickyDropParticle;


    [Header("CONFIGURATIONS")]
    [SerializeField]
    private LayerMask _groundLayer, _platformLayer;
    [SerializeField]
    private float _particleDistance = 0.3f;

    private const float VERTICAL_RAYCAST_LENGHT = 0.2F;

    private Animator _animator;
    private Rigidbody2D _rigidBody2D;
    private PlayerStatus _playerStatus;
    private PlayerDamageDealer _playerDamageDealer;

    private bool _isOnTheGround = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _hitboxCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        if (_isOnTheGround) return;

        _isOnTheGround = IsOnTheGround();

        if (_isOnTheGround)
        {
            TouchedTheGround();
        }
        else if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(MyAnimations.Dropping.ToString()))
        {
            _animator.Play(MyAnimations.Dropping.ToString());
        }
    }

    public void SetInitialValues(PlayerStatus playerStatus, PlayerDamageDealer playerDamageDealer)
    {
        _playerStatus = playerStatus;
        _playerDamageDealer = playerDamageDealer;

        _hitbox.OnHitboxTriggerEnter.AddListener(HandleEffectOnTHeGround);
    }

    private void HandleEffectOnTHeGround(Hitbox targetHitbox, Hitbox myHitbox)
    {
        _playerDamageDealer.OnApplyPoopDamage(targetHitbox, myHitbox, () => { });
    }

    private void TouchedTheGround()
    {
        _hitboxCollider.enabled = true;
        StartCoroutine(Vibrate());

        _animator.Play(MyAnimations.Floor.ToString());
        _rigidBody2D.velocity = Vector3.zero;
        _rigidBody2D.isKinematic = true;

        if (_playerStatus.Poop.AreaOfEffect.Get() == 1 || _playerStatus.Poop.AreaOfEffect.Get() == 0) return;

        _hitboxTransform.localScale = new Vector3(_playerStatus.Poop.AreaOfEffect.Get(), 1, 1);

        Vector2 leftPosition = this.transform.position;
        Vector2 rightPosition = this.transform.position;
        for (int i = 1; i < _playerStatus.Poop.AreaOfEffect.Get(); i++)
        {
            leftPosition = new Vector2(leftPosition.x - _particleDistance, leftPosition.y);
            rightPosition = new Vector2(rightPosition.x + _particleDistance, rightPosition.y);
            SpawnAreaPartcile(leftPosition);
            SpawnAreaPartcile(rightPosition);
        }
    }

    private void SpawnAreaPartcile(Vector2 leftPosition)
    {
        PoopStickyDropParticle particle = Instantiate(_stickyDropParticle, leftPosition, Quaternion.identity);
        particle.transform.parent = this.transform;
    }

    private bool IsOnTheGround()
    {
        bool isOnTheGroundInternal = _auxiliarPositions.All(e => IsHittingLayer(_groundLayer, e.position) || IsHittingLayer(_platformLayer, e.position));

        if(!isOnTheGroundInternal && _auxiliarPositions.Any(e => IsHittingLayer(_groundLayer, e.position) || IsHittingLayer(_platformLayer, e.position))) 
        {
            StartCoroutine(UnstuckFromOddPosition());
        }

        return isOnTheGroundInternal;
    }

    private bool IsHittingLayer(LayerMask layer, Vector2 position)
    {
        RaycastHit2D col = Physics2D.Linecast(position, new Vector2(position.x, position.y + VERTICAL_RAYCAST_LENGHT), layer);
        return col.collider != null;
    }

    private IEnumerator Vibrate()
    {
        float cdw = 0;

        Vector2 velocity = new Vector2(0.001f, 0);

        while (cdw <= _playerStatus.Poop.Duration.Get())
        {
            cdw += Time.deltaTime;

            yield return new WaitForFixedUpdate();

            _rigidBody2D.velocity = velocity;
        }

        Destroy(this.gameObject);
    }

    private enum MyAnimations
    {
        Dropping,
        Floor
    }

    private IEnumerator UnstuckFromOddPosition()
    {
        if (IsNotMoving())
        {
            yield return new WaitForSeconds(0.5f);

            if (IsNotMoving())
            {
                _isOnTheGround = true;
                TouchedTheGround();
            }
        }
    }
    private bool IsNotMoving() => _rigidBody2D.velocity == Vector2.zero;
}
