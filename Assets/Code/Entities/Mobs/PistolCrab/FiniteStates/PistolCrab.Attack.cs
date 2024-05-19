using System.Collections;
using UnityEngine;

public partial class PistolCrab : Enemy
{
    private class Attack : PistolCrabBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Attack;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _pistolCrab.StartCoroutine(ResetAttackCdw());
        }

        public override void OnEnterBehaviour()
        {
            _pistolCrab.RotationalTransform.localScale = new Vector3(1, 1, 1);

            _attackModel.OnShootLeftPistol.AddListener(ShootLeftPistol);
            _attackModel.OnShootRightPistol.AddListener(ShootRightPistol);

            _attackModel.CanAttack = false;
            _pistolCrab.StartCoroutine(PermformAttack());
        }

        public override void OnExitBehaviour()
        {
            _attackModel.OnShootLeftPistol.RemoveListener(ShootLeftPistol);
            _attackModel.OnShootRightPistol.RemoveListener(ShootRightPistol);

            _pistolCrab.StartCoroutine(ResetAttackCdw());
        }

        public override void Update()
        {
        }

        private void ShootLeftPistol()
        {
            PistolCrabProjectile projectile = SpawnProjectile(_attackModel.LeftPistolPosition.position);
            ShootPistol(new Vector2(-0.5f, 0.5f), projectile);
        }

        private void ShootRightPistol()
        {
            PistolCrabProjectile projectile = SpawnProjectile(_attackModel.RightPistolPosition.position);
            ShootPistol(new Vector2(0.5f, 0.5f), projectile);
        }

        private PistolCrabProjectile SpawnProjectile(Vector2 position)
        {
            PistolCrabProjectile projectile = Instantiate(_attackModel.ProjectilePrefab, position, Quaternion.identity);
            projectile.transform.parent = null;
            return projectile;
        }

        private void ShootPistol(Vector2 normalizedDirection, PistolCrabProjectile projectile)
        {
            _pistolCrab.DamageDealer.OnRegisterProjectileEvent.Invoke(projectile);
            projectile.Shoot(normalizedDirection * _attackModel.ProjectileSpeed.GetRandom());
        }

        private IEnumerator ResetAttackCdw()
        {
            yield return new WaitForSeconds(_attackModel.CdwToAttack);
            _attackModel.CanAttack = true;
        }

        private IEnumerator PermformAttack()
        {
            _pistolCrab.Animator.PlayAnimation(PistolCrabAnimatorModel.AnimationName.PistolCrab_Attack);
            yield return new WaitForSeconds(_attackModel.AttackDuration);

            _pistolCrab.ChangeBehaviour(Behaviour.Idle);
        }
    }
}