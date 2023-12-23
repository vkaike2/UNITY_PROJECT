using UnityEngine;

public class TurtleGun : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Transform _rotationalTransform;
    [SerializeField]
    private TurtleDamageDealer _damageDealer;
    [SerializeField]
    private Turtle _turtle;

    [Header("PREFAB")]
    [SerializeField]
    private TurtleProjectile _projectile;

    [Header("SHOOT POSITION")]
    [SerializeField]
    private Transform _rightPosition;
    [SerializeField]
    private Transform _leftPosition;

    private Animator _animator;

    private const string SHOOT_ANIMATOR_STRING = "TurtleGun_Shoot";
    private float _projectileSpeed;
    private float _duration;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Shoot(float projectileSpeed, float duration)
    {
        _projectileSpeed = projectileSpeed;
        _duration = duration;
        _animator.Play(SHOOT_ANIMATOR_STRING);
    }

    public void ANIMATOR_OnShootFrame()
    {
        TurtleProjectile rightProjectile = Instantiate(_projectile, _rightPosition.transform.position, Quaternion.identity);
        TurtleProjectile leftProjectile = Instantiate(_projectile, _leftPosition.transform.position, Quaternion.identity);

        _damageDealer.OnRegisterProjectileEvent.Invoke(rightProjectile);
        _damageDealer.OnRegisterProjectileEvent.Invoke(leftProjectile);

        rightProjectile.transform.parent = null;
        leftProjectile.transform.parent = null;

        Vector2 normalizedRightSpeed = new Vector2(1f, 1f);
        Vector2 normalizedLeftSpeed = new Vector2(-1f, 1f);

        Vector2 rightSpeed = normalizedRightSpeed * _projectileSpeed;
        Vector2 leftSpeed = normalizedLeftSpeed * _projectileSpeed;

        if (_rotationalTransform.localScale.x == 1)
        {
            rightProjectile.SetInitialInitialValues(rightSpeed, _duration);
            leftProjectile.SetInitialInitialValues(leftSpeed, _duration);
        }
        else
        {
            rightProjectile.SetInitialInitialValues(leftSpeed, _duration);
            leftProjectile.SetInitialInitialValues(rightSpeed, _duration);
        }

        _turtle.WalkModel.OnRestartShoot.Invoke();
    }
}