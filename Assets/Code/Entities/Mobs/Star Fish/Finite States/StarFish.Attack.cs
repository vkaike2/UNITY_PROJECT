using System.Collections;
using UnityEngine;

partial class StarFish : Enemy
{
    private class Attack : StarFishBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Attack;

        private const float CDW_TO_GO_IDLE_AFTER_SHOOT = 0.3f;

        public override void OnEnterBehaviour()
        {
            _attackModel.OnSpawnProjectile.AddListener(OnSpawnProjectile);

            LookAtPlayer();

            _starFish.Animator.PlayAnimation(StarFishAnimatorModel.AnimationName.Star_Fish_Attack);
        }

        public override void OnExitBehaviour()
        {
            _attackModel.OnSpawnProjectile.RemoveListener(OnSpawnProjectile);
        }

        public override void Update()
        {
        }

        private void LookAtPlayer()
        {
            Player player = _starFish.GameManager.Player;

            bool needToLookToRight = player.transform.position.x > _starFish.transform.position.x;

            _starFish.RotationalTransform.localScale = new Vector3(needToLookToRight ? 1 : -1, 1, 1);
        }

        private void OnSpawnProjectile()
        {
            StarFishProjectile projectile = Instantiate(
                 _attackModel.StarFishProjectilePrefab,
                 _attackModel.ProjectileSpawnPostion.position,
                 Quaternion.identity);
            _starFish.DamageDealer.OnRegisterProjectileEvent.Invoke(projectile);

            projectile.transform.parent = null;

            projectile.ShootProjectile(_attackModel.ProjectileSpeed, _starFish.RotationalTransform.localScale.x == 1);


            _starFish.StartCoroutine(WaitThenGoIdle());
        }

        private IEnumerator WaitThenGoIdle()
        {
            yield return new WaitForSeconds(CDW_TO_GO_IDLE_AFTER_SHOOT);
            _starFish.ChangeBehaviour(Behaviour.Idle);
        }
    }
}