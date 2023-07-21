using UnityEngine;


public partial class Rat : Enemy
{
    private class Idle : RatBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Idle;

        public override void OnEnterBehaviour()
        {
            _rat.RatAnimator.PlayAnimation(RatAnimatorModel.AnimationName.Rat_Idle);
        }

        public override void OnExitBehaviour()
        {
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
            return Vector2.Distance(playerPosition, _rat.transform.position) <= _rat.IdleModel.DistanceToStartFollowingPlayer;
        }
    }

}
