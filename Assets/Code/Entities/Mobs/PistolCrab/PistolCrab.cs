using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class PistolCrab : Enemy
{

    [field: Header("MY COMPONENTS")]
    [field: SerializeField]
    public PistolCrabAnimatorModel Animator { get; private set; }

    [field: Header("MODELS")]
    [field: SerializeField]
    public PistolCrabIdleModel IdleModel { get; private set; }
    [field: SerializeField]
    public PistolCrabWalkModel WalkModel { get; private set; }
    [field: SerializeField]
    public PistolCrabAttackModel AttackModel { get; private set; }

    protected PistolCrabDamageDealer DamageDealer { get; private set; }
    public Behaviour CurrentBehaviour { get; set; }

    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();

    private List<PistolCrabBaseBehaviour> _finiteBaseBehaviours = new List<PistolCrabBaseBehaviour>()
    {
        new Idle(),
        new Walk(),
        new Attack(),
        new Die()
    };

    private void OnDrawGizmos()
    {
        WalkModel.WillHitTheWallCheck.DrawGizmos();
        WalkModel.WillHitTheGround.DrawGizmos();
    }

    protected override void AfterAwake()
    {
        base.AfterAwake();
        DamageDealer = GetComponent<PistolCrabDamageDealer>();
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

    public void ANIMATOR_ShootPistol(PistolCrabAnimatorEvents.Direction direction)
    {
        switch (direction)
        {
            case PistolCrabAnimatorEvents.Direction.Left:
                AttackModel.OnShootLeftPistol.Invoke();
                break;
            case PistolCrabAnimatorEvents.Direction.Right:
                AttackModel.OnShootRightPistol.Invoke();
                break;
        }
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
        Attack,
        Die
    }
}