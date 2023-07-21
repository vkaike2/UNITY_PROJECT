using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Egg : Enemy
{
    [field: Header("MY CONFIGURATIONS")]
    [field: SerializeField]
    public EggAnimatorModel EggAnimator { get; private set; }
    [field: Space]
    [field: SerializeField]
    public EggSpawningModel SpawningModel { get; private set; }
    [field: SerializeField]
    public EggIdleModel IdleModel { get; private set; }

    public Behaviour? CurrentBehaviour => ((EggBaseBehaviour)_currentFiniteBehaviour)?.Behaviour;
    
    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();

    private readonly List<EggBaseBehaviour> _finiteBaseBehaviours = new List<EggBaseBehaviour>()
    {
        new Idle(),
        new Spawning(),
        new Die()
    };

    protected override void AfterStart()
    {
        ChangeBehaviour(Behaviour.Idle);
    }

    public void ChangeBehaviour(Behaviour behaviour)
    {
        _currentFiniteBehaviour?.OnExitBehaviour();

        _currentFiniteBehaviour = _finiteBaseBehaviours.FirstOrDefault(e => e.Behaviour == behaviour);

        _currentFiniteBehaviour.OnEnterBehaviour();
    }

    public enum Behaviour
    {
        Idle,
        Spawning,
        Die
    }
}