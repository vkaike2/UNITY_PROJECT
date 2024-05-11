using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class StarFish : Enemy
{
    [field: Header("UI COMPONENTS")]
    [field: SerializeField]
    public CdwIndicationUI CdwIndicationUI { get; set; }

    [field: Header("MY COMPONENTS")]
    [field: SerializeField]
    public StarFishAnimatorModel Animator { get; set; }

    [field: Header("MODELS")]
    [field: SerializeField]
    public StarFishIdleModel IdleModel { get; set; }
    [field: SerializeField]
    public StarFishAttckModel AttackModel { get; set; }

    public Behaviour CurrentBehaviour { get; set; }

    protected StarFishDamageDealer DamageDealer { get; private set; }

    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();

    private List<StarFishBaseBehaviour> _finiteBaseBehaviours = new List<StarFishBaseBehaviour>()
    {
        new Born(),
        new Idle(),
        new Attack(),
        new Die()
    };

    protected override void AfterAwake()
    {
        DamageDealer = GetComponent<StarFishDamageDealer>();
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

    public void ANIMATOR_SpawnProjecile()
    {
        AttackModel.OnSpawnProjectile.Invoke();
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
        Attack,
        Die
    }
}