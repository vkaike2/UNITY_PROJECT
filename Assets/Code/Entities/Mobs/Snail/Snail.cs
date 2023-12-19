using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Snail : Enemy
{
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public RaycastModel GroundCheck { get; private set; }
    [field: SerializeField]
    public Rigidbody2D RigidBody2D { get; private set; }
    [field: SerializeField]
    public SnailAnimatorModel Animator { get; private set; }

    [field: Header("MODELS")]
    [field: SerializeField]
    public SnailWalkingModel WalkingModel { get; private set; }

    public Behaviour? CurrentBehaviour => ((SnailBaseBehaviour)_currentFiniteBehaviour)?.Behaviour;
    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();


    private List<SnailBaseBehaviour> _finiteBaseBehaviours = new List<SnailBaseBehaviour>()
    {
        new Walking(),
        new Die()
    };

    private void OnDrawGizmos()
    {
        GroundCheck.DrawGizmos();
    }

    protected override void AfterStart()
    {
        base.AfterStart();
        GroundCheck.GetInitialValues();
    }

    #region ANIMATOR EVENTS
    public void ANIMATOR_SetInitialBehaviour()
    {
        ChangeBehaviour(Behaviour.Walking);
    }
    #endregion


    public override void Kill()
    {
        ChangeBehaviour(Behaviour.Die);
    }

    public void ChangeBehaviour(Behaviour behaviour)
    {
        _currentFiniteBehaviour?.OnExitBehaviour();
        _currentFiniteBehaviour = _finiteBaseBehaviours.FirstOrDefault(e => e.Behaviour == behaviour);
        _currentFiniteBehaviour.OnEnterBehaviour();
    }

    public bool IsCollidingWithGround()
    {
        return GroundCheck.DrawPhysics2D(WalkingModel.LayerToCheck) != null;
    }

    public enum Behaviour
    {
        Born,
        Walking,
        Die
    }
}
