using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public partial class Armadillo : MonoBehaviour
{
    [field: Header("EVENT")]
    [field: SerializeField]
    public ScriptableCameraEvents CameraEvents { get; private set; }

    [field: Header("UI COMPONENTS")]
    [field: SerializeField]
    protected CdwIndicationUI CdwIndicationUI { get; private set; }

    [field: Header("MY COMPONENTS")]
    [field: SerializeField]
    public ArmadilloAnimatorModel MainAnimator { get; private set; }
    [field: SerializeField]
    public Transform RotationalTransform { get; private set; }
    [field: SerializeField]
    public EnemyStatus Status { get; private set; }
    [field: SerializeField]
    protected MyColliders Colliders { get; set; }

    [field: Header("MY MODELS")]
    [field: SerializeField]
    public ArmadilloRunningTowardsWallModel RunningTowardsWallModel { get; set; }
    [field: SerializeField]
    public ArmadilloShockwaveModel ShockwaveModel { get; set; }
    [field: SerializeField]
    public ArmadilloIntoBallModel IntoBallModel { get; set; }
    [field: SerializeField]
    public ArmadilloIdleModel IdleModel { get; set; }

    public ArmadilloBaseBehaviour CurrentBaseBehaviour { get; private set; }

    protected UnityEvent OnFinishSwpawnAnimation { get; private set; } = new UnityEvent();

    protected UnityEvent OnSpawnShockwaveAnimation { get; private set; } = new UnityEvent();
    protected GameManager GameManager { get; private set; }
    protected ArmadilloDamageDealer DamageDealer { get; private set; }
    protected ArmadilloDamageReceiver DamageReceiver { get; private set; }


    private readonly List<ArmadilloBaseBehaviour> _behaviours = new List<ArmadilloBaseBehaviour>()
    {
        new Spawning(),
        new Idle(),
        new RunTowardsWall(),
        new Shockwave(),
        new IntoBall(),
        new Death()
    };

    private void Awake()
    {
        DamageDealer = this.GetComponent<ArmadilloDamageDealer>();
        DamageReceiver = this.GetComponent<ArmadilloDamageReceiver>();
        Colliders.DamageDealer = DamageDealer;
        Colliders.DamageReceiver = DamageReceiver;
    }

    private void Start()
    {
        Colliders.ActivateCollider(MyColliders.ColliderType.Main);
        GameManager = GameObject.FindObjectOfType<GameManager>();
        foreach (var behaviour in _behaviours)
        {
            behaviour.Start(this);
        }

        ChangeBehaviour(Behaviour.Spawning);
    }

    private void FixedUpdate()
    {
        if (CurrentBaseBehaviour == null) return;

        CurrentBaseBehaviour.Update();
    }

    #region CALLED BY ANIMATOR EVENTS
    public void ANIMATOR_OnFinishJumpAnimation()
    {
        OnFinishSwpawnAnimation.Invoke();
    }

    public void ANIMATOR_OnSpawnShockwaveAnimation()
    {
        OnSpawnShockwaveAnimation.Invoke();
    }
    #endregion

    public void ChangeBehaviour(Behaviour behaviour)
    {
        CurrentBaseBehaviour?.OnExitBehaviour();

        CurrentBaseBehaviour = _behaviours.FirstOrDefault(e => e.Behaviour == behaviour);
        CurrentBaseBehaviour.OnEnterBehaviour();
    }

    public enum Behaviour
    {
        Spawning,
        Idle,
        RunTowardsWall,
        Shockwave,
        IntoBall,
        Death
    }

    [Serializable]
    public class MyColliders
    {
        [field: Header("COLLIDERS")]
        [field: SerializeField]
        public BoxCollider2D MainCollider { get; set; }
        [field: SerializeField]
        public BoxCollider2D HitWallCollider { get; set; }
        [field: SerializeField]
        public CircleCollider2D IntoBallCollider { get; set; }

        [field: Header("HITBOX")]
        [field: SerializeField]
        public Hitbox MainHitbox { get; set; }
        [field: SerializeField]
        public Hitbox IntoBallHitbox { get; set; }
        [field: SerializeField]
        public Hitbox HitAllHitbox { get; set; }

        public ArmadilloDamageDealer DamageDealer { get; set; }
        public ArmadilloDamageReceiver DamageReceiver { get; set; }

        public void ActivateCollider(ColliderType type)
        {

            MainCollider.enabled = false;
            HitWallCollider.enabled = false;
            IntoBallCollider.enabled = false;

            DeactivateEveryHitbox();
            switch (type)
            {
                case ColliderType.Main:
                    MainCollider.enabled = true;
                    MainHitbox.GetComponent<Collider2D>().enabled = true;
                    
                    DamageDealer.OnChangeHitbox.Invoke(MainHitbox);
                    DamageReceiver.OnChangeHitbox.Invoke(MainHitbox);
                    break;
                case ColliderType.HitWall:
                    HitWallCollider.enabled = true;
                    HitAllHitbox.GetComponent<Collider2D>().enabled = true;

                    DamageDealer.OnChangeHitbox.Invoke(HitAllHitbox);
                    DamageReceiver.OnChangeHitbox.Invoke(HitAllHitbox);
                    break;
                case ColliderType.Ball:
                    IntoBallCollider.enabled = true;
                    IntoBallHitbox.GetComponent<Collider2D>().enabled = true;

                    DamageDealer.OnChangeHitbox.Invoke(IntoBallHitbox);
                    DamageReceiver.OnChangeHitbox.Invoke(IntoBallHitbox);
                    break;
            }
        }

        public void DeactivateEveryHitbox()
        {
            MainHitbox.GetComponent<Collider2D>().enabled = false;
            IntoBallHitbox.GetComponent<Collider2D>().enabled = false;
            HitAllHitbox.GetComponent<Collider2D>().enabled = false;
        }

        public enum ColliderType
        {
            Main,
            HitWall,
            Ball
        }
    }
}