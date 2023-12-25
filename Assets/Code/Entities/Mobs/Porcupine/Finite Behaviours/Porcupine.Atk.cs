using System.Collections;
using UnityEngine;


public partial class Porcupine : Enemy
{
    public class Atk : PorcupineBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Atk;

        private PorcupineAtkModel _atkModel;
        private PorcupineDamageDealer _damageDealer;

        private Coroutine _cdwCoroutine;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);

            _damageDealer = _porcupine.GetComponent<PorcupineDamageDealer>();
            _atkModel = _porcupine.AtkModel;
            _porcupine.AnimatorEvents.OnTriggerAtkProjectile.AddListener(OnTriggerAtkProjectile);
            _porcupine.AnimatorEvents.OnStartAtkJump.AddListener(OnStartAtkJump);
            _porcupine.AnimatorEvents.OnEndAtk.AddListener(OnEndAnimation);
            _atkModel.CanAtk = true;
        }

        public override void OnEnterBehaviour()
        {
            // is dead
            if (!_atkModel.CanAtk)
            {
                return;
            }

            _porcupine.CanMove = false;
            _rigidbody2D.velocity = Vector3.zero;
            _porcupine.PorculineAnimator.PlayAnimation(PorcupineAnimatorModel.AnimationName.Atk);
        }

        public override void OnExitBehaviour() 
        {
        }

        public override void Update() { }

        private void OnStartAtkJump()
        {
            _rigidbody2D.velocity = new Vector2(0, _atkModel.JumpForce);
        }

        private void OnTriggerAtkProjectile()
        {
            // facing left
            PorcupineProjectile leftProjectile = GameObject.Instantiate(_atkModel.Projectile, _atkModel.ProjectileSpawnPoint.position, Quaternion.identity, _atkModel.ProjectileContainer);
            leftProjectile.SetInitialInitialValues(new Vector2(_atkModel.ProjectileSpeed * -0.5f, _atkModel.ProjectileSpeed), _atkModel.ProjectileDuration);
            _damageDealer.OnRegisterProjectileEvent.Invoke(leftProjectile);

            // facing up
            PorcupineProjectile upProjectile = GameObject.Instantiate(_atkModel.Projectile, _atkModel.ProjectileSpawnPoint.position, Quaternion.identity, _atkModel.ProjectileContainer);
            upProjectile.SetInitialInitialValues(new Vector2(0, _atkModel.ProjectileSpeed), _atkModel.ProjectileDuration);
            _damageDealer.OnRegisterProjectileEvent.Invoke(upProjectile);

            // facing right
            PorcupineProjectile rightProjectile = GameObject.Instantiate(_atkModel.Projectile, _atkModel.ProjectileSpawnPoint.position, Quaternion.identity, _atkModel.ProjectileContainer);
            rightProjectile.SetInitialInitialValues(new Vector2(_atkModel.ProjectileSpeed * 0.5f, _atkModel.ProjectileSpeed), _atkModel.ProjectileDuration);
            _damageDealer.OnRegisterProjectileEvent.Invoke(rightProjectile);
        }

        private void OnEndAnimation()
        {
            _cdwCoroutine = _porcupine.StartCoroutine(CountCdwBetweenAtks());
            _porcupine.StartCoroutine(WaitTouchGroundThenChangeToPatrol());
        }

        private IEnumerator WaitTouchGroundThenChangeToPatrol()
        {
            while (!_atkModel.GroundCheckRaycast.IsRaycastCollidingWithLayer)
            {
                yield return new WaitForFixedUpdate();
            }

            _porcupine.ChangeBehaviour(Porcupine.Behaviour.Patrol);
        }

        private IEnumerator CountCdwBetweenAtks()
        {
            _atkModel.CanAtk = false;
            _porcupine.CdwIndicationUI.StartCdw(_atkModel.CdwBetweenAtks);
            yield return new WaitForSeconds(_atkModel.CdwBetweenAtks);
            Debug.Log("VOLTO PRA FALSE");
            _atkModel.CanAtk = true;
        }
    }

}
