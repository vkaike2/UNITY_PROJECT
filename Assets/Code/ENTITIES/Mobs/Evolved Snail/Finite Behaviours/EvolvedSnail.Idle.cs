using System.Collections;
using UnityEngine;

public partial class EvolvedSnail : Enemy
{
    private class Idle : EvolvedSnailBaseBhaviour
    {
        public override Behaviour Behaviour => EvolvedSnail.Behaviour.Idle;

        private EvolvedSnailIdleModel _idleModel;

        private Coroutine _checkIfStillIdleCoroutine;
        private bool _checkIfPlayerIsReady = false;

        private const float TIME_TO_CREATE_LEG = 1f;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            _idleModel = _evolvedSnail.IdleModel;
            _evolvedSnail.StartCoroutine(WaitGameObjectToBeReady());
        }

        public override void OnEnterBehaviour()
        {
            _evolvedSnail.Animator.PlayAnimation(EvolvedSnailAnimatorModel.AnimationName.Evolved_Snail_Idle);
            _checkIfStillIdleCoroutine = _evolvedSnail.StartCoroutine(CheckIfImStillIdle());
        }

        public override void OnExitBehaviour()
        {
            CheckIfShouldStopCoroutine();
            _idleModel.CdwIndicationUI.ForceEndCdw();
        }

        public override void Update()
        {
            if (CheckIfShouldFollowPlayer())
            {
                _evolvedSnail.StartCoroutine(PrepareToFollowPlayer());
            }
        }

        private void CheckIfShouldStopCoroutine()
        {
            if (_checkIfStillIdleCoroutine == null) return;
            _evolvedSnail.StopCoroutine(_checkIfStillIdleCoroutine);
        }

        private bool CheckIfShouldFollowPlayer()
        {
            if (!_checkIfPlayerIsReady)
            {
                return false;
            }

            Vector3 playerPosition = _evolvedSnail.GameManager.Player.transform.position;
            return Vector2.Distance(playerPosition, _evolvedSnail.transform.position) <= _idleModel.RangeToStartFollowingPlayer;
        }

        /// <summary>
        ///     Will start following player if its in idle for too much of a time
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckIfImStillIdle()
        {
            _idleModel.CdwIndicationUI.StartCdw(_idleModel.CdwToStartFollowingPlayer);
            yield return new WaitForSeconds(_idleModel.CdwToStartFollowingPlayer);
            _evolvedSnail.StartCoroutine(PrepareToFollowPlayer());
        }

        private IEnumerator WaitGameObjectToBeReady()
        {
            yield return new WaitUntil(() => _gameManager.Player != null);
            _checkIfPlayerIsReady = true;
        }

        private IEnumerator PrepareToFollowPlayer()
        {
            _evolvedSnail.Animator.PlayAnimation(EvolvedSnailAnimatorModel.AnimationName.Evolved_Snail_Creating_Leg);
            yield return new WaitForSeconds(TIME_TO_CREATE_LEG);
            _evolvedSnail.ChangeBehaviour(Behaviour.FollowingPlayer);
        }
    }
}