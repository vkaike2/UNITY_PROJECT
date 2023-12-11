using System;
using System.Collections;
using UnityEngine;


public partial class GunGaroo : MonoBehaviour
{
    private class Shoot : GunGarooBaseBehaviour
    {
        private GunGarooContainer _container;
        private GunGarooShootModel _shootModel;
        private GunGarooDamageDealer _damageDealer;

        public override Behaviour Behaviour => Behaviour.Shoot;

        private Coroutine _waitToLeaveState;

        public override void Start(GunGaroo gunGaroo)
        {
            base.Start(gunGaroo);

            _container = gunGaroo.container;
            _shootModel = gunGaroo.ShootModel;
            _damageDealer = gunGaroo.GetComponent<GunGarooDamageDealer>();
        }

        public override void OnEnterBehaviour()
        {
            _gunGaroo.OnShootFrame.AddListener(ShootBullet);

            _gunGaroo.MainAnimator.PlayAnimation(GunGarooAnimatorModel.AnimationName.Shoot);
            _waitToLeaveState = _gunGaroo.StartCoroutine(WaitToLeaveState());
        }

        public override void OnExitBehaviour()
        {
            _gunGaroo.OnShootFrame.RemoveListener(ShootBullet);
            
            if (_waitToLeaveState != null) _gunGaroo.StopCoroutine(_waitToLeaveState);
        }

        public override void Update()
        {

        }

        private void ShootBullet()
        {
            GunGarooBullet bulletPrefab = GameObject.Instantiate(_shootModel.BulletPrefab, _shootModel.ShootPosition.position, Quaternion.identity);

            _damageDealer.OnRegisterBullet.Invoke(bulletPrefab);

            Vector2 playerPosition = _gunGaroo.gameManager.Player.transform.position;

            Vector2 velocity = new Vector2(
                    playerPosition.x > _gunGaroo.transform.position.x ? _shootModel.BulletSpeed : -_shootModel.BulletSpeed,
                    0);

            bulletPrefab.SetInitialInitialValues(velocity);

        }

        private IEnumerator WaitToLeaveState()
        {
            yield return new WaitForSeconds(_shootModel.CdwToLeaveState);
            _gunGaroo.ChangeBehaviour(Behaviour.Idle);
        }
    }
}
