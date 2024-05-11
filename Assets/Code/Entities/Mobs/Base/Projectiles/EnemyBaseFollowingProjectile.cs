using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),
                  typeof(LayerCheckCollider))]
public class EnemyBaseFollowingProjectile : MonoBehaviour
{
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _initialSpeed = 8;
    [SerializeField]
    private float _cdwToStartFollowingPlayer = 1;
    [SerializeField]
    private MinMax _horizontalAxisVelocity;
    [SerializeField]
    [Tooltip("Will try to Aim on player First")]
    private bool _tryToUseNormalTrajectory = true;

    private Rigidbody2D _rigidbody2D;
    private LayerCheckCollider _layerCheckCollider;
    private GameManager _gameManager;

    private void OnValidate()
    {
        if (_horizontalAxisVelocity.Min == 0 && _horizontalAxisVelocity.Max == 0)
        {
            _horizontalAxisVelocity = new MinMax(-1, 1);
        }
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _layerCheckCollider = GetComponent<LayerCheckCollider>();
    }

    private void Start()
    {
        _layerCheckCollider.OnLayerCheckTriggerEnter.AddListener(OnCollidingWithGround);
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void ShootProjectile(float projectileSpeed, bool isFacingRight)
    {
        StartCoroutine(WaitUntilGameManagerIsReady(() =>
        {
            Vector2 playerPosition = _gameManager.Player.transform.position;
            Vector2 projectileJumpSpeed = MovementUtils.CalculateJumpVelocity(playerPosition, this.transform.position, 1);

            if (IsNaN(projectileJumpSpeed) || !_tryToUseNormalTrajectory)
            {
                StartCoroutine(ManageProjectileTrajectory(projectileSpeed, isFacingRight));
                return;
            }

            _rigidbody2D.velocity = projectileJumpSpeed;
        }));
    }

    private void OnCollidingWithGround(GameObject collidingWith)
    {
        Destroy(this.gameObject);
    }

    private bool IsNaN(Vector2 vector)
    {
        return float.IsNaN(vector.x) || float.IsNaN(vector.y);
    }

    private IEnumerator ManageProjectileTrajectory(float projectileSpeed, bool isFacingRight)
    {
        Vector2 normalizedSpeed = new Vector2(_horizontalAxisVelocity.GetRandom(), isFacingRight ? 1 : 1);
        _rigidbody2D.velocity = normalizedSpeed * _initialSpeed;

        yield return new WaitForSeconds(_cdwToStartFollowingPlayer);

        _rigidbody2D.gravityScale = 0;

        Vector2 playerPosition = _gameManager.Player.transform.position;
        Vector2 direction = (playerPosition - (Vector2)this.transform.position).normalized;
        _rigidbody2D.velocity = direction * projectileSpeed;
    }

    private IEnumerator WaitUntilGameManagerIsReady(Action callback)
    {
        yield return new WaitUntil(() => _gameManager != null && _gameManager.Player != null);
        callback();
    }
}