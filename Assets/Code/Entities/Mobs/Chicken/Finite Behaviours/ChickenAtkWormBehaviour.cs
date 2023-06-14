using System.Collections;
using UnityEngine;


public class ChickenAtkWormBehaviour : ChickenFiniteBaseBehaviour
{
    public override Chicken.Behaviour Behaviour => Chicken.Behaviour.Atk_Worm;

    private ChickenAtkWormBehaviourModel _atkWormModel;

    public override void OnEnterBehaviour()
    {
        _chicken.Animator.PlayAnimation(ChickenAnimatorModel.AnimationName.MeleeAtk);
    }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);

        _atkWormModel = _chicken.AtkWormModel;

        _atkWormModel.InteractWithWorm = () => InteractWithWorm();
        _atkWormModel.EndAtkAnimation = () => GoToPatrolBehaviour();
    }

    public override void OnExitBehaviour()
    {
    }

    public override void Update()
    {
    }

    private void InteractWithWorm()
    {
        _atkWormModel.WormTarget.InteractWithChicken();
        _chicken.AddTier();
    }

    private void GoToPatrolBehaviour()
    {
        _chicken.ChangeBehaviour(Chicken.Behaviour.Patrol);
    }

}
