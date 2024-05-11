using UnityEngine;

public class ScorpionTail : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Scorpion _scorpion;
    [SerializeField]
    private ScorpionDamgeDealer _damageDealer;

    private Animator _animator;

    private const string OPEN_TAIL_ANIMATION_STRING = "Scorpion_Tail_Shoot";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    public void StartShoot()
    {
        _animator.Play(OPEN_TAIL_ANIMATION_STRING);
    }

    public void ANIMATOR_SHOOT_FRAME()
    {
        ScorpionProjectile projectile = Instantiate(_scorpion.AttackModel.Projectile, this.transform.position, Quaternion.identity);
        _damageDealer.OnRegisterProjectileEvent.Invoke(projectile);

        projectile.transform.parent = null;
        projectile.ShootProjectile(
            _scorpion.AttackModel.ProjectileSpeed,
            _scorpion.RotationalTransform.localScale.x == 1);

        _scorpion.AttackModel.OnReadyToShootAgain.Invoke();
    }
}