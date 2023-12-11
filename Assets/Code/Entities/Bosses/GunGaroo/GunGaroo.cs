using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public partial class GunGaroo : MonoBehaviour
{
    [Header("UI COMPONENTS")]
    [SerializeField]
    protected CdwIndicationUI cdwIndicationUI;
    [SerializeField]
    protected GunGarooNextAttackIndicationUI nextAttackIndicationUI;

    [field: Header("VFX")]
    [field: SerializeField]
    public GunGarooLandingVFX LandingVFX { get; private set; }

    [Header("COMPONENTS")]
    [SerializeField]
    protected Transform rotationalTransform;
    [field: SerializeField]
    public EnemyStatus Status { get; private set; }

    [field: Header("MY COMPONENTS")]
    [field: SerializeField]
    public GunGarooAnimatorModel MainAnimator { get; private set; }

    [field: Header("CHECKS")]
    [field: SerializeField]
    public LayerCheckCollider GroundCheck { get; private set; }

    [field: Header("MODELS")]
    [field: SerializeField]
    public GunGarooIdleModel IdleModel {  get; private set; }
    [field: SerializeField]
    public GunGarooJumpModel JumpModel { get; private set; }
    [field: SerializeField]
    public GunGarooShootModel ShootModel { get; private set; }
    [field: SerializeField]
    public GunGarooDeathModel DeathModel { get; private set; }

    public GunGarooBaseBehaviour CurrentBaseBehaviour { get; private set; }

    protected UnityEvent OnJumpFrame { get; private set; } = new UnityEvent();
    protected UnityEvent OnShootFrame { get; private set; } = new UnityEvent();

    protected GunGarooContainer container;
    protected GameManager gameManager;

    private readonly List<GunGarooBaseBehaviour> _behaviours = new List<GunGarooBaseBehaviour>()
    {
        new Spawning(),
        new Idle(),
        new JumpToOtherSide(),
        new JumpToThePlayer(),
        new Shoot(),
        new SuperJump(),
        new Death()
    };

    private Rigidbody2D _rigidBody2D;

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        foreach (var behaviour in _behaviours)
        {
            behaviour.Start(this);
        }
    }

    private void OnValidate()
    {
        IdleModel.Validate();
    }

    private void FixedUpdate()
    {
        foreach (var behaviour in _behaviours)
        {
            behaviour.Update();
        }
    }

    public void AddInitialValues(GunGarooContainer gunGarooContainer, bool flipPlayerToTheRight)
    {
        container = gunGarooContainer;

        FlipGunGaroo(flipPlayerToTheRight);
        FreezeRigidBodyConstraints(true);

        StartCoroutine(WaitBehavioursToBeStarted(() =>
        {
            Debug.Log("Started");
            this.ChangeBehaviour(Behaviour.Spawning);
        }));
    }

    public void ChangeBehaviour(Behaviour behaviour)
    {
        CurrentBaseBehaviour?.OnExitBehaviour();

        CurrentBaseBehaviour = _behaviours.FirstOrDefault(e => e.Behaviour == behaviour);
        CurrentBaseBehaviour.OnEnterBehaviour();
    }

    public void FreezeRigidBodyConstraints(bool freeze)
    {
        if (freeze)
        {
            _rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            _rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public GunGarooLandingVFX SpawnLandingVFX(Vector2 position)
    {
        GunGarooLandingVFX vfx = GameObject.Instantiate(LandingVFX, position, Quaternion.identity);
        vfx.transform.SetParent(container.transform);
        return vfx;
    }

    #region CALLED BY ANIMATOR EVENTS
    public void ANIMATOR_JumpFrame()
    {
        OnJumpFrame.Invoke();
    }

    public void ANIMATOR_ShootFrame()
    {
        OnShootFrame.Invoke();
    }
    #endregion

    private void FlipGunGaroo(bool toTheRight)
    {
        rotationalTransform.localScale = new Vector3(
            toTheRight ? 1 : -1, 1, 1);
    }

    private IEnumerator WaitBehavioursToBeStarted(Action callback)
    {
        yield return new WaitUntil(() => _behaviours.Count(e => e.IsStarted) == _behaviours.Count);
        callback();
    }

    public enum Behaviour
    {
        Spawning,
        Idle,
        JumpToOtherSide,
        JumpToThePlayer,
        Shoot,
        SuperJump,
        Death
    }
}
