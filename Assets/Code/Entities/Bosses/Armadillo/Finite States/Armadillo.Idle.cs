using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Armadillo : MonoBehaviour
{
    private class Idle : ArmadilloBaseBehaviour
    {
        public override Behaviour Behaviour => Behaviour.Idle;

        private List<Attack> _attacks = new List<Attack>(){
            new Attack(Behaviour.RunTowardsWall, 1),
            new Attack(Behaviour.Shockwave, 0.7f),
            new Attack(Behaviour.IntoBall, 0.5f),
        };

        public override void OnEnterBehaviour()
        {
            _mainAnimator.PlayAnimation(ArmadilloAnimatorModel.AnimationName.IDLE);

            _armadillo.StartCoroutine(TestOtherStates());
        }

        public override void OnExitBehaviour()
        {
        }

        public override void Update()
        {
        }

        private IEnumerator TestOtherStates()
        {
            _armadillo.CdwIndicationUI.StartCdw(_idleModel.CdwToChangeState);
            yield return new WaitForSeconds(_idleModel.CdwToChangeState);

            if (_armadillo.CurrentBaseBehaviour.Behaviour != Behaviour.Death)
            {
                _armadillo.ChangeBehaviour(GetNextBehaviour());
            }
        }


        private Behaviour GetNextBehaviour()
        {
            float lifePercentage = _armadillo.Status.Health.Get() / _armadillo.Status.MaxHealth.Get();
            List<Attack> possibleAttacks = _attacks.Where(e => e.LifePercentageToBeValid >= lifePercentage).ToList();

            Debug.Log($"lifePercentage: {lifePercentage} | {string.Join(", ", possibleAttacks.Select(e => $"{e.Behaviour},{e.LifePercentageToBeValid}").ToList())}");


            return possibleAttacks[UnityEngine.Random.Range(0, possibleAttacks.Count)].Behaviour;
        }


        public class Attack
        {
            public Attack(Behaviour behaviour, float lifePercentageToBeValid)
            {
                Behaviour = behaviour;
                LifePercentageToBeValid = lifePercentageToBeValid;
            }

            public Behaviour Behaviour { get; set; }
            public float LifePercentageToBeValid { get; set; }
        }
    }
}