using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class FlyingFish : Enemy
{
    [field: Header("MY COMPONENTS")]
    [field: SerializeField]
    public FlyingFishAnimatorModel Animator { get; private set; }

    [field: Header("MODELS")]
    [field: SerializeField]
    public FlyingFishIdleModel IdleModel { get; set; }
    [field: SerializeField]
    public FlyingFishWalkModel WalkModel { get; set; }
     [field: SerializeField]
    public FlyingFishAttackModel AttackModel { get; set; }

    public Behaviour CurrentBehaviour { get; set; }
    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();
    protected WaterSection WaterSection { get; private set; }


    private List<FlyingFishBaseBehaviour> _finiteBaseBehaviours = new List<FlyingFishBaseBehaviour>()
    {
        new Idle(),
        new Walk(),
        new Attack(),
        new Floor(),
        new Die()
    };

    protected override void AfterStart()
    {
        WaterSection = GameObject.FindObjectOfType<WaterSection>();
    }

    public override void Kill()
    {
        ChangeBehaviour(Behaviour.Die);
    }

    #region ANIMATOR EVENTS
    public void ANIMATOR_SetInitialBehaviour()
    {
        ChangeBehaviour(Behaviour.Idle);
    }

    public void ANIMATOR_StartMoving()
    {
        WalkModel.OnStartMoving.Invoke();
    }
    #endregion


    public void ChangeBehaviour(Behaviour behaviour)
    {
        _currentFiniteBehaviour?.OnExitBehaviour();
        _currentFiniteBehaviour = _finiteBaseBehaviours.FirstOrDefault(e => e.Behaviour == behaviour);

        _currentFiniteBehaviour.OnEnterBehaviour();
        CurrentBehaviour = behaviour;
    }

    public enum Behaviour
    {
        Born,
        Idle,
        Walk,
        Floor,
        Attack,
        Die
    }
}