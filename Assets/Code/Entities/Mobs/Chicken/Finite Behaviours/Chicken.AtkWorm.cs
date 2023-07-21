using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Chicken : Enemy
{
    [field: SerializeField]
    public ChickenAtkWormModel AtkWormModel { get; private set; }
    public class AtkWorm : ChickenBaseBehaviour
    {
        public override Behaviour Behaviour => Chicken.Behaviour.Atk_Worm;
        private ChickenAtkWormModel _atkWormModel;

        public override void OnEnterBehaviour()
        {
            _chicken.ChickenAnimator.PlayAnimation(ChickenAnimatorModel.AnimationName.MeleeAtk);

            _atkWormModel.InteractWithWorm = () => OnInteractWithWorm();
            _atkWormModel.EndAtkAnimation = () => OnEndAtkAnimation();

            _atkWormModel.ThrowingEgg = () => ThrowEgg();
        }

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);

            _atkWormModel = _chicken.AtkWormModel;

            ResetInternalActions();
        }

        public override void OnExitBehaviour()
        {
            ResetInternalActions();
        }

        private void ResetInternalActions()
        {
            _atkWormModel.InteractWithWorm = () => { };
            _atkWormModel.EndAtkAnimation = () => { };
        }

        public override void Update() { }

        private void OnInteractWithWorm()
        {
            _atkWormModel.WormTarget.InteractWithChicken();
            _chicken.AddTier();
        }

        private void OnEndAtkAnimation()
        {
            _chicken.ChickenAnimator.PlayAnimation(ChickenAnimatorModel.AnimationName.ThrowingEgg);
        }

        private void ThrowEgg()
        {
            bool isFacingRight = _chicken.RotationalTransform.localScale.x == 1;

            Egg egg = GameObject.Instantiate(_atkWormModel.ChickensEgg, _atkWormModel.SpawnerPosition);
            egg.GetComponent<Rigidbody2D>().velocity = new Vector2(isFacingRight ? -_atkWormModel.EggVelocity : _atkWormModel.EggVelocity, 2);
            egg.transform.parent = null;

            _chicken.StartCoroutine(WaitThenGoToPatrol());
        }

        private IEnumerator WaitThenGoToPatrol()
        {
            yield return new WaitForSeconds(0.5F);

            _chicken.ChangeBehaviour(Chicken.Behaviour.Patrol);
        }
    }

}
