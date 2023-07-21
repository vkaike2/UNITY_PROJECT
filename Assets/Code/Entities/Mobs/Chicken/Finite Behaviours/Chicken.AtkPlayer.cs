using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Chicken : Enemy
{
    [field: SerializeField]
    public ChickenAtkPlayerModel AtkPlayerModel { get; private set; }

    public class AtkPlayer : ChickenBaseBehaviour
    {
        public override Chicken.Behaviour Behaviour => Chicken.Behaviour.Atk_Player;

        private ChickenAtkPlayerModel _atkPlayerModel;
        private Player _player;

        public override void OnEnterBehaviour()
        {
            _chicken.ChickenAnimator.PlayAnimation(ChickenAnimatorModel.AnimationName.MeleeAtk);

            _atkPlayerModel.InteractWithPlayer = () => InteractWithPlayer();
            _atkPlayerModel.EndAtkAnimation = () => EndAtkAnimator();
        }

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);

            _atkPlayerModel = _chicken.AtkPlayerModel;

            _atkPlayerModel.AtkHitbox.Collider.enabled = false;
            _player = _chicken.GameManager.Player;

            ResetInternalActions();
        }

        public override void OnExitBehaviour()
        {
            ResetInternalActions();
        }

        public override void Update()
        {
        }

        private void ResetInternalActions()
        {
            _atkPlayerModel.InteractWithPlayer = () => { };
            _atkPlayerModel.EndAtkAnimation = () => { };
        }

        private void InteractWithPlayer()
        {
            _atkPlayerModel.AtkHitbox.Collider.enabled = true;
        }

        private void EndAtkAnimator()
        {
            _atkPlayerModel.AtkHitbox.Collider.enabled = false;

            GoToPatrolBehaviour();
        }

        private void GoToPatrolBehaviour()
        {
            _chicken.ChangeBehaviour(Chicken.Behaviour.Patrol);
        }
    }
}
