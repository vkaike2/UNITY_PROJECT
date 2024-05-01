using System.Collections;
using UnityEngine;

public partial class Armadillo : MonoBehaviour
{
    private class Shockwave : ArmadilloBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Shockwave;

        public override void OnEnterBehaviour()
        {
            LookForPlayerDirection();

            _armadillo.MainAnimator.PlayAnimation(ArmadilloAnimatorModel.AnimationName.SHOCKWAVE);
            _armadillo.OnSpawnShockwaveAnimation.AddListener(SpawnShockWave);

            _armadillo.StartCoroutine(WaitThenChangeToIdle());
        }

        public override void OnExitBehaviour()
        {
            _armadillo.OnSpawnShockwaveAnimation.RemoveListener(SpawnShockWave);
        }

        public override void Update()
        {
        }

        private void SpawnShockWave()
        {
            var shockwaveParent = GameObject.Instantiate(
                _shockwaveModel.ShockwaveParentPrefab,
                 _shockwaveModel.PositionToStartShockwave.transform.position, Quaternion.identity);

            shockwaveParent.transform.parent = null;

            shockwaveParent.SpawnShockwave((ShockwaveParent.Direction)_currentDirection, _armadillo.DamageDealer);
        }

        private void LookForPlayerDirection()
        {
            Player player = _armadillo.GameManager.Player;
            Direction direction = _armadillo.transform.position.x > player.transform.position.x ? Direction.Left : Direction.Right;

            RotateArmadillo(direction);
        }

        private IEnumerator WaitThenChangeToIdle()
        {
            yield return new WaitForSeconds(_shockwaveModel.StateDuration);
            _armadillo.ChangeBehaviour(Behaviour.Idle);
        }
    }
}