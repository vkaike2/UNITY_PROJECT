using System.Collections;
using UnityEngine;


public partial class Rat : Enemy
{
    private class Idle : RatBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Idle;

        private Coroutine _checkIfStillIdleCoroutine;

        private RatIdleModel _idleModel;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _idleModel = _rat.IdleModel;
        }

        public override void OnEnterBehaviour()
        {
            _rat.RatAnimator.PlayAnimation(RatAnimatorModel.AnimationName.Rat_Idle);
            _checkIfStillIdleCoroutine = _rat.StartCoroutine(CheckIfImStillIdle());
        }

        public override void OnExitBehaviour()
        {
            CheckIfShouldStopCoroutine();
            _idleModel.CdwIndicationUI.ForceEndCdw();
        }

        private void CheckIfShouldStopCoroutine()
        {
            if (_checkIfStillIdleCoroutine == null) return;
            _rat.StopCoroutine(_checkIfStillIdleCoroutine);
        }

        public override void Update()
        {
            if (CheckIfShouldFollowPlayer())
            {
                _rat.ChangeBehaviour(Behaviour.FollowingPlayer);
            }
        }

        private bool CheckIfShouldFollowPlayer()
        {
            Vector3 playerPosition = _rat.GameManager.Player.transform.position;
            return Vector2.Distance(playerPosition, _rat.transform.position) <= _idleModel.DistanceToStartFollowingPlayer;
        }

        /// <summary>
        ///     Will start following player if its in idle for too much of a time
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckIfImStillIdle()
        {
            _idleModel.CdwIndicationUI.StartCdw(_idleModel.CdwToStartFollowingPlayer);
            yield return new WaitForSeconds(_idleModel.CdwToStartFollowingPlayer);
            _rat.ChangeBehaviour(Behaviour.FollowingPlayer);
        }

    }
}
