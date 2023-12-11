using System.Collections;
using UnityEngine;


public partial class GunGaroo : MonoBehaviour
{
    private class Death : GunGarooBaseBehaviour
    {
        private GunGarooDeathModel _deathModel;

        public override Behaviour Behaviour => Behaviour.Death;

        public override void Start(GunGaroo gunGaroo)
        {
            base.Start(gunGaroo);
            _deathModel = gunGaroo.DeathModel;
        }

        public override void OnEnterBehaviour()
        {
            _deathModel.Hitbox.gameObject.GetComponent<Collider2D>().enabled = false;

            _rigidBody2D.velocity = new Vector2(0, -5);
            _gunGaroo.MainAnimator.PlayAnimation(GunGarooAnimatorModel.AnimationName.Death);
        }

        public override void OnExitBehaviour()
        {
        }

        public override void Update()
        {
        }
    }
}
