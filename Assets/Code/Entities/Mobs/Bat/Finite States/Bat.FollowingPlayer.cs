using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Bat : Enemy
{
    private class FollowingPlayer : BatBaseBehaviour
    {
        public override Behaviour Behaviour => Bat.Behaviour.FollowingPlayer;

        private Vector2 _playerPosition;
        private float _playerDistance;

        public override void OnEnterBehaviour()
        {
            _bat.OnFlapWings.AddListener(OnFlapWings);
            _bat.Animator.PlayAnimation(BatAnimatorModel.AnimationName.Bat_Flying);
        }

        public override void OnExitBehaviour()
        {
            _bat.OnFlapWings.RemoveListener(OnFlapWings);
            ResetVelocity();
        }

        public override void Update()
        {
            UpdatePlayerInfo();
            if (_playerDistance <= DISTANCE_TO_STOP_FOLLOW) return;
            UpdateSpeedToFollowTarget(_playerPosition);
        }

        private void UpdatePlayerInfo()
        {
            _playerPosition = _gameManager.Player.transform.position;
            _playerDistance = Vector2.Distance(_playerPosition, _bat.transform.position);
        }
    }
}
