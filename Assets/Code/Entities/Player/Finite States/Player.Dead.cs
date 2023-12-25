using System.Collections;
using UnityEngine;


public partial class Player : MonoBehaviour
{
    private class Dead : PlayerBaseState
    {
        public override FiniteState State => FiniteState.Dead;

        public override void EnterState()
        {
            _rigidBody2D.velocity = new Vector2(0, _rigidBody2D.velocity.y);

            _player.StartCoroutine(WaitItTouchGroundThenMakeItStatic());
        }

        public override void OnExitState()
        {
        }

        public override void Update()
        {
        }

        private IEnumerator WaitItTouchGroundThenMakeItStatic()
        {
            while (!IsPlayerTouchingGround)
            {
                yield return new WaitForFixedUpdate();
            }

            _rigidBody2D.bodyType = RigidbodyType2D.Static;

            _animator.ClearHightPriorityAnimation(_player);
            _animator.PlayAnimation(PlayerAnimatorModel.Animation.Die);

            _player.GameManager.OnPlayerDead.Invoke(_player.DieModel.DamageSourceThatKilledYou);
        }
    }
}
