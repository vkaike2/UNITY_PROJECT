using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GunGaroo : MonoBehaviour
{
    private class Spawning : GunGarooBaseBehaviour
    {
        public override Behaviour Behaviour => GunGaroo.Behaviour.Spawning;

        private const float TIME_TO_RUN_SPAWNING_GROUND_ANIMATION = 1.2f;

        public override void OnEnterBehaviour()
        {
            _gunGaroo.StartCoroutine(StartSpawnProcess());
        }

        public override void OnExitBehaviour()
        {
        }

        public override void Update()
        {
        }

        private IEnumerator StartSpawnProcess()
        {
            _gunGaroo.MainAnimator.PlayAnimation(GunGarooAnimatorModel.AnimationName.Spawning);
            _gunGaroo.FreezeRigidBodyConstraints(false);

            yield return new WaitUntil(() => _gunGaroo.GroundCheck.IsRaycastCollidingWithLayer);

            _gunGaroo.MainAnimator.PlayAnimation(GunGarooAnimatorModel.AnimationName.Spawning_Ground);

            yield return new WaitForSeconds(TIME_TO_RUN_SPAWNING_GROUND_ANIMATION);

            _gunGaroo.ChangeBehaviour(Behaviour.Idle);
        }
    }
}
