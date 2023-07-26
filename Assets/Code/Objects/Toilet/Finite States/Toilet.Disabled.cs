using System.Collections;
using UnityEngine;

public partial class Toilet : MonoBehaviour
{
    private class Disabled : ToiletStateBase
    {
        public override FiniteState State => FiniteState.Disabled;

        private Toilet _toilet;

        public override void Start(Toilet toilet)
        {
            _toilet = toilet;
        }
        public override void Update()
        {
        }

        public override void EnterState()
        {
            _toilet.PlatformCollider.SetActive(false);
            _toilet.CanInteractWithPlayer = false;
            _toilet.Animator.Play(Toilet.MyAnimations.Disabled.ToString());

        }

        public override void OnExitState()
        {
        }
    }
}
