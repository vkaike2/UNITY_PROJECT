using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class GunGaroo : MonoBehaviour
{
    private class Idle : GunGarooBaseBehaviour
    {
        public override Behaviour Behaviour => GunGaroo.Behaviour.Idle;

        private GunGarooIdleModel _model;
        private GunGarooIdleModel.NextAttack _lastAttack = null;
        public override void Start(GunGaroo gunGaroo)
        {
            base.Start(gunGaroo);
            _model = gunGaroo.IdleModel;
        }

        public override void OnEnterBehaviour()
        {
            _gunGaroo.MainAnimator.PlayAnimation(GunGarooAnimatorModel.AnimationName.Idle);

            _gunGaroo.StartCoroutine(WaitCdwToGoToNextAttack());
        }

        public override void OnExitBehaviour()
        {
        }

        public override void Update()
        {
        }

        private IEnumerator WaitCdwToGoToNextAttack()
        {
            GunGarooIdleModel.NextAttack attack = GetNextAttack();

            float cdw = attack.CdwToStart.GetRandom();
            _gunGaroo.nextAttackIndicationUI.SetSpriteFor(attack.NextAttackIndication, cdw);
            _gunGaroo.cdwIndicationUI.StartCdw(cdw);
            yield return new WaitForSeconds(cdw);

            Debug.Log($"next - {attack.NextBehaviour}");

            _gunGaroo.ChangeBehaviour(attack.NextBehaviour);
        }

        private GunGarooIdleModel.NextAttack GetNextAttack()
        {
            List<GunGarooIdleModel.NextAttack> iterationList = _model.AttackList;
            if (_lastAttack != null)
            {
                iterationList = iterationList.Where(e => _lastAttack.Attack != e.Attack).ToList();

            }
            int totalWeight = iterationList.Sum(i => i.Weight);


            int random = UnityEngine.Random.Range(0, totalWeight);
            int count = 0;

            foreach (var attack in iterationList)
            {
                count += attack.Weight;

                if(count >= random)
                {
                    _lastAttack = attack;
                    return attack;
                }
            }

            return _model.AttackList.FirstOrDefault();
        }

    }
}
