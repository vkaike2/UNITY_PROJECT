using System.Collections;
using UnityEngine;


public partial class Player : MonoBehaviour
{
    public class Eating : PlayerBaseState
    {
        public override FiniteState State => FiniteState.Eating;

        private PlayerEatModel _eatModel;

        public override void Start(Player player)
        {
            base.Start(player);
            _eatModel = _player.EatModel;
        }

        public override void EnterState()
        {
            _animator.PlayAnimation(PlayerAnimatorModel.Animation.Eat);
            _player.StartCoroutine(WaitAnimationDuration());
            
            _rigidBody2D.velocity = Vector3.zero;
        }

        public override void OnExitState()
        {
            _eatModel.EatingItemData = null;
        }

        public override void Update()
        {
        }

        private IEnumerator WaitAnimationDuration()
        {
            float halfCdw = _eatModel.CdwToMoveOutState / 2;

            yield return new WaitForSeconds(halfCdw);
            _playerInventory.EatItem(_eatModel.EatingItemData);

            yield return new WaitForSeconds(halfCdw);

            if(_player.MoveInput.Value != Vector2.zero)
            {

                ChangeState(FiniteState.Move);
            }
            else
            {
                ChangeState(FiniteState.Idle);
            }
        }

    }
}
