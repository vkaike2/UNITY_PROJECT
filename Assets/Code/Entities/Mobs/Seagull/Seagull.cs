using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Seagull : Enemy
{
    [field: Header("MY COMPONENTS")]
    [field: SerializeField]
    public SeagullAnimatorModel Animator { get; set; }
    [field: SerializeField]
    public List<Collider2D> MainColliders { get; set; }

    [field: Header("MODELS")]
    [field: SerializeField]
    public SeagullFlyModel FlyModel { get; set; }
    [field: SerializeField]
    public SeagullGroundModel GroundModel { get; set; }

    public Behaviour CurrentBehaviour { get; set; }

    protected bool IsFlying = true;
    protected SeagullDamageDealer DamageDealer { get; set; }
    protected SkySection SkySection { get; private set; }
    protected override List<EnemyBaseBehaviour> FiniteBaseBehaviours => _finiteBaseBehaviours.Select(e => (EnemyBaseBehaviour)e).ToList();

    private List<SeagullBaseBehaviour> _finiteBaseBehaviours = new List<SeagullBaseBehaviour>()
    {
        new Fly(),
        new Ground(),
        new Die()
    };

    protected override void AfterAwake()
    {
        base.AfterAwake();
        DamageDealer = GetComponent<SeagullDamageDealer>();
    }

    protected override void AfterStart()
    {
        base.AfterStart();
        SkySection = GameObject.FindObjectOfType<SkySection>();

        foreach (var collider in MainColliders)
        {
            collider.enabled = false;
        }
    }

    public override void Kill()
    {
        ChangeBehaviour(Behaviour.Die);
    }

    #region ANIMATOR EVENTS
    public void ANIMATOR_SetInitialBehaviour()
    {
        ChangeBehaviour(Behaviour.Fly);
    }

    public void ANIMATOR_FlappingWings()
    {
        FlyModel.OnFlappingWings.Invoke();
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
        Fly,
        Ground,
        Die
    }
}