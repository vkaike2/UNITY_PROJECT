
using System.Collections;
using UnityEngine;

public partial class EvolvedSnail : Enemy
{
    private class Attack : EvolvedSnailBaseBhaviour
    {
        public override Behaviour Behaviour => Behaviour.Atk_Player;

        private EvolvedSnailAttackModel _attackModel;
        private EvolvedSnailDamageDealer _damageDelaer;

        private float _initialGravity;
        private const float TIME_FOR_ATK_ANIMATION = 1.7f;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _damageDelaer = _evolvedSnail.GetComponent<EvolvedSnailDamageDealer>();
            _initialGravity = _rigidbody2D.gravityScale;
            _attackModel = _evolvedSnail.AttackModel;
        }

        public override void OnEnterBehaviour()
        {
            _attackModel.OnAttackFrame.AddListener(OnAttackFrame);
            _evolvedSnail.StartCoroutine(PrepareAndAttack());
        }

        public override void OnExitBehaviour()
        {
            _attackModel.OnAttackFrame.RemoveListener(OnAttackFrame);
            _rigidbody2D.gravityScale = _initialGravity;
        }

        public override void Update()
        {
        }

        private IEnumerator PrepareAndAttack()
        {
            _rigidbody2D.gravityScale = _initialGravity * _attackModel.PercentageGravityMultiplier;
            _rigidbody2D.velocity = Vector2.zero;

            Vector2 playerPosition = _evolvedSnail.GameManager.Player.transform.position;

            bool needToFaceRight = playerPosition.x > _evolvedSnail.transform.position.x;

            Vector3 _initialLocalScale = _evolvedSnail.RotationalTransform.localScale;
            _evolvedSnail.RotationalTransform.localScale = new Vector3(needToFaceRight ? -1 : 1, 1, 1);

            _evolvedSnail.Animator.PlayAnimation(EvolvedSnailAnimatorModel.AnimationName.Evolved_Snail_Attack);

            yield return new WaitForSeconds(TIME_FOR_ATK_ANIMATION);

            _attackModel.CanAttack = false;
            _evolvedSnail.StartCoroutine(ResetCooldown());

            _evolvedSnail.RotationalTransform.localScale = _initialLocalScale;
            _evolvedSnail.ChangeBehaviour(Behaviour.Idle);
        }

        private void OnAttackFrame()
        {
            EvolvedSnailProjectile projectile = GameObject.Instantiate(_attackModel.Projectile, _attackModel.ButtPosition.position, Quaternion.identity);
            _damageDelaer.OnRegisterProjectileEvent.Invoke(projectile);
            
            Vector2 playerPosition = _evolvedSnail.GameManager.Player.transform.position;
            Vector2 direction = (playerPosition - (Vector2)projectile.transform.position).normalized;

            projectile.SetInitialInitialValues(direction * _attackModel.ProjectileSpeed);
        }

        private IEnumerator ResetCooldown()
        {
            yield return new WaitForSeconds(_attackModel.CdwBetweenEachAttack);
            _attackModel.CanAttack = true;
        }
    }
}